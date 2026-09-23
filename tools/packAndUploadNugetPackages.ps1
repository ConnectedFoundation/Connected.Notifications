#Replace YOUR-GITHUB-PAT with your personal github token.
#Replace PROJECT_NAME with project folder
#Duplicate last line for all projects with packages
#See https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry
# 
dotnet pack --configuration Release
dotnet nuget push "PROJECT_NAME\bin\Release\*.nupkg" --skip-duplicate --api-key YOUR_GITHUB_PAT --source "github"