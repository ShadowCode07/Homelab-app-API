#!/usr/bin/env bash
# Scan the Homelab API with the local SonarQube (see docker-compose.sonar.yml).
# Run from the root of Homelab-app-API:   SONAR_TOKEN=<your token> ./sonar-local.sh
set -euo pipefail

: "${SONAR_TOKEN:?Set SONAR_TOKEN first, e.g. SONAR_TOKEN=sqp_xxx ./sonar-local.sh}"
PROJECT_KEY="Homelab-app-API"
HOST="http://localhost:9000"

# Old coverage files would be counted again, so start clean
rm -rf ./*/TestResults

dotnet sonarscanner begin \
  /k:"$PROJECT_KEY" \
  /d:sonar.host.url="$HOST" \
  /d:sonar.token="$SONAR_TOKEN" \
  /d:sonar.cs.opencover.reportsPaths="**/TestResults/*/coverage.opencover.xml" \
  /d:sonar.exclusions="**/Migrations/**"

dotnet build HomelabAPI.sln --no-incremental

dotnet test HomelabAPI.sln --no-build \
  --collect:"XPlat Code Coverage" \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover

dotnet sonarscanner end /d:sonar.token="$SONAR_TOKEN"

echo "Done: open $HOST/dashboard?id=$PROJECT_KEY"
