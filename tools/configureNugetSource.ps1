#Replace YOUR-GITHUB-PAT with your personal github token.
#See https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-nuget-registry
# 
dotnet nuget add source --username Tom-PIT --password YOUR_GITHUB_PAT --store-password-in-clear-text --name github "https://nuget.pkg.github.com/Tom-PIT/index.json"