# Wordle Clone Wars
A site to compare your Wordle (and various clones) scores against other users.
Supports 4 different clones right now:
* [Wordle](https://www.nytimes.com/games/wordle/index.html)
* [Ordlig](https://ordlig.se/)
* [Ordsnille](https://ordsnille.brusman.se/statistik)
* [Nerdle](https://nerdlegame.com/)
* [Ordel](https://ordel.se/)

If you'd like some others please open an issue.

## Local configuration with user secrets

For `local` and `loacal` environments, email and Syncfusion settings should come from .NET user secrets.

From `src/WordleCloneWars` run:

```bash
dotnet user-secrets set "EmailSettings:ApiKey" "<sendgrid-api-key>"
dotnet user-secrets set "EmailSettings:FromEmail" "<from-email>"
dotnet user-secrets set "SyncfusionKey" "<syncfusion-license-key>"
```

The app already has a `UserSecretsId` in the project file, so these values will be picked up automatically when running with the `local` or `loacal` launch profile.
