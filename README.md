<div align="center">
  <h1>SwiftlyS2 Team Identity Manager</h1>
  <p>Randomly assigns esports team logos and names on map load</p>
</div>

<p align="center">
  <a href="https://developer.valvesoftware.com/wiki/Source_2"><img src="https://img.shields.io/badge/Source%202-orange?style=for-the-badge&logo=valve&logoColor=white" alt="Source 2"></a>
  <a href="https://github.com/zhw1nq/Team-Identity-Manager/releases"><img src="https://img.shields.io/badge/Version-1.0.0-blue?style=for-the-badge" alt="Version"></a>
  <a href="https://dotnet.microsoft.com/"><img src="https://img.shields.io/badge/.NET-10.0-purple?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET"></a>
</p>

---

## Showcase

<div align="center">
  <img src="Showcase.png" alt="Showcase"/>
  <p><i>Logo Team và Team Name được đặt ở sau điểm số của Site, mọi thứ khác đều không liên quan.</i></p>
</div>

---

## Platform Support

| Platform | Status    |
| -------- | --------- |
| Windows  | Ready     |
| Linux    | Not Ready |

## Requirements

- [SwiftlyS2](https://github.com/swiftly-solution/swiftlys2) v1.0.0+

## Installation

1. Download the [latest release](https://github.com/zhw1nq/Team-Identity-Manager/releases)
2. Extract plugin to `addons/swiftlys2/plugins/Team-Identity-Manager/`
3. Restart the server
4. Configure `config.jsonc` if needed

## Configuration

| Option            | Type   | Default | Description                          |
| ----------------- | ------ | ------- | ------------------------------------ |
| `DebugMode`       | bool   | `false` | Enable debug logging                 |
| `RandomTeamLogos` | bool   | `true`  | Enable random team logos on map load |
| `RandomTeamNames` | bool   | `true`  | Enable random team names on map load |
| `CtTeamLogo`      | string | `""`    | Fixed CT team logo code              |
| `CtTeamName`      | string | `""`    | Fixed CT team name                   |
| `TTeamLogo`       | string | `""`    | Fixed T team logo code               |
| `TTeamName`       | string | `""`    | Fixed T team name                    |

## Building from Source

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SwiftlyS2.CS2 NuGet Package](https://www.nuget.org/packages/SwiftlyS2.CS2)

### Build

```bash
git clone https://github.com/zhw1nq/Team-Identity-Manager.git
cd Team-Identity-Manager
dotnet restore
dotnet build
```

### Publish

```bash
dotnet publish -c Release
```

Output directory: `build/publish/Team-Identity-Manager/`

## Credits

- [zhw1nq](https://github.com/zhw1nq) - Author
- [SwiftlyS2](https://github.com/swiftly-solution/swiftlys2) - Framework

## License

This project is licensed under the MIT License.