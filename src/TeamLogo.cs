using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared.Events;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared;
using System.Text.Json.Serialization;

namespace TeamIdentityManager;

public class TeamIdentityConfig
{
    [JsonPropertyName("DebugMode")]
    public bool DebugMode { get; set; } = false;

    [JsonPropertyName("RandomTeamLogos")]
    public bool RandomTeamLogos { get; set; } = true;

    [JsonPropertyName("RandomTeamNames")]
    public bool RandomTeamNames { get; set; } = true;

    [JsonPropertyName("CtTeamName")]
    public string CtTeamName { get; set; } = "";

    [JsonPropertyName("CtTeamLogo")]
    public string CtTeamLogo { get; set; } = "";

    [JsonPropertyName("TTeamName")]
    public string TTeamName { get; set; } = "";

    [JsonPropertyName("TTeamLogo")]
    public string TTeamLogo { get; set; } = "";
}

[PluginMetadata(Id = "Team-Identity-Manager", Version = "1.0.0", Name = "Team Identity Manager", Author = "zhw1nq", Description = "Randomly assigns esports team logos and names on map load")]
public partial class TeamIdentityManager : BasePlugin
{
    private static readonly Dictionary<string, string> TeamNames = new()
    {
        { "zzn", "00 Nation" }, { "thv", "100 Thieves" }, { "3dm", "3DMAX" }, { "nein", "9INE" },
        { "pand", "9Pandas" }, { "nine", "9z Team" }, { "amka", "AMKAL ESPORTS" }, { "apex", "Apeks" },
        { "ad", "Astana Dragons" }, { "astr", "Astralis" }, { "avg", "Avangar" }, { "bne", "Bad News Eagles" },
        { "big", "BIG" }, { "bravg", "Bravado Gaming" }, { "cm", "Clan-Mystik" }, { "c9", "Cloud9" },
        { "col", "compLexity Gaming" }, { "cope", "Copenhagen Flames" }, { "cw", "Copenhagen Wolves" },
        { "clg", "Counter Logic Gaming" }, { "cr4z", "CR4ZY" }, { "dat", "dAT Team" }, { "dig", "Team Dignitas" },
        { "drea", "DreamEaters" }, { "ebet", "Team eBettle" }, { "ecst", "ECSTATIC" }, { "ence", "ENCE eSports" },
        { "ent", "Entropiq" }, { "nv", "Team EnVyUs" }, { "eps", "Epsilon eSports" }, { "esc", "ESC Gaming" },
        { "eter", "Eternal Fire" }, { "evl", "Evil Geniuses" }, { "faze", "FaZe Clan" }, { "flg", "Flash Gaming" },
        { "flip", "Flipsid3 Tactics" }, { "flux", "Fluxo" }, { "fq", "FlyQuest" }, { "fntc", "Fnatic" },
        { "forz", "FORZE Esports" }, { "furi", "FURIA Esports" }, { "g2", "G2 Esports" }, { "gamb", "Gambit Esports" },
        { "gl", "Team GamerLegion" }, { "god", "GODSENT" }, { "gray", "Grayhound Gaming" }, { "hlr", "HellRaisers" },
        { "hero", "Heroic" }, { "ibp", "Team iBUYPOWER" }, { "ihc", "IHC Esports" }, { "imt", "Immortals" },
        { "im", "Team Immunity" }, { "imp", "Imperial Esports" }, { "itb", "Into The Breach" }, { "intz", "INTZ eSports" },
        { "keyd", "Keyd Stars" }, { "king", "Team Kinguin" }, { "koi", "KOI" }, { "ldlc", "Team LDLC.com" },
        { "lgcy", "Legacy" }, { "lgb", "LGB eSports" }, { "liq", "Team Liquid" }, { "lc", "London Conspiracy" },
        { "lumi", "Luminosity Gaming" }, { "lynn", "Lynn Vision Gaming" }, { "mibr", "MIBR" }, { "mfg", "Misfits" },
        { "mngz", "The MongolZ" }, { "mont", "Monte" }, { "mss", "mousesports" }, { "mouz", "MOUZ" },
        { "ride", "Movistar Riders" }, { "myxmg", "myXMG" }, { "nf", "n!faculty" }, { "navi", "Natus Vincere" },
        { "nip", "Ninjas in Pyjamas" }, { "nor", "North" }, { "nrg", "NRG Esports" }, { "og", "OG" },
        { "optc", "OpTic Gaming" }, { "orbit", "Orbit Esport" }, { "out", "Outsiders" }, { "pain", "paiN Gaming" },
        { "psnu", "Passion UA" }, { "penta", "PENTA Sports" }, { "pkd", "Planetkey Dynamics" },
        { "qb", "Quantum Bellator Fire" }, { "ratm", "Rare Atom" }, { "rgg", "Reason Gaming" },
        { "wgg", "Recursive eSports" }, { "ren", "Renegades" }, { "rog", "Rogue" }, { "saw", "SAW" },
        { "shrk", "Sharks Esports" }, { "sk", "SK Gaming" }, { "spc", "Space Soldiers" }, { "spir", "Team Spirit" },
        { "splc", "Splyce" }, { "spr", "Sprout" }, { "syma", "Syman Gaming" }, { "tsm", "Team SoloMid" },
        { "tit", "Titan" }, { "tyl", "Tyloo" }, { "us", "Universal Soldiers" }, { "vega", "Vega Squadron" },
        { "vg", "VeryGames" }, { "vex", "Vexed Gaming" }, { "vici", "ViCi Gaming" }, { "vp", "Virtus.Pro" },
        { "vita", "Team Vitality" }, { "ve", "Vox Eminor" }, { "wins", "Winstrike Team" }, { "wcrd", "Wildcard" },
        { "indw", "Team Wolf" }, { "e6ten", "x6tence" }, { "xapso", "Xapso" }
    };

    private static readonly string[] TeamLogos = TeamNames.Keys.ToArray();
    private TeamIdentityConfig _config = new();
    private EventDelegates.OnMapLoad? _onMapLoadHandler;

    public TeamIdentityManager(ISwiftlyCore core) : base(core) { }

    public override void ConfigureSharedInterface(IInterfaceManager interfaceManager) { }

    public override void UseSharedInterface(IInterfaceManager interfaceManager) { }

    public override void Load(bool hotReload)
    {
        LoadConfiguration();
        _onMapLoadHandler = OnMapLoad;
        Core.Event.OnMapLoad += _onMapLoadHandler;
        if (hotReload) Core.Scheduler.NextTick(ApplyTeamLogos);
        LogDebug("Plugin loaded successfully");
    }

    public override void Unload()
    {
        if (_onMapLoadHandler is not null)
        {
            Core.Event.OnMapLoad -= _onMapLoadHandler;
            _onMapLoadHandler = null;
        }
        LogDebug("Plugin unloaded");
    }

    private void LoadConfiguration()
    {
        Core.Configuration
            .InitializeWithTemplate("config.jsonc", "config.template.jsonc")
            .Configure(builder => builder.AddJsonFile("config.jsonc", optional: false, reloadOnChange: true));

        var services = new ServiceCollection();
        services.AddSwiftly(Core)
            .AddOptionsWithValidateOnStart<TeamIdentityConfig>()
            .BindConfiguration("TeamIdentityManager");

        using var provider = services.BuildServiceProvider();
        _config = provider.GetRequiredService<IOptionsMonitor<TeamIdentityConfig>>().CurrentValue;
        LogDebug("Configuration loaded");
    }

    private void OnMapLoad(IOnMapLoadEvent @event)
    {
        LogDebug("Map loaded: {MapName}", @event.MapName);
        Core.Scheduler.NextTick(ApplyTeamLogos);
    }

    private void ApplyTeamLogos()
    {
        string logoCT = _config.CtTeamLogo;
        string nameCT = _config.CtTeamName;
        string logoT = _config.TTeamLogo;
        string nameT = _config.TTeamName;

        if (_config.RandomTeamLogos)
        {
            if (string.IsNullOrEmpty(logoCT)) logoCT = TeamLogos[Random.Shared.Next(TeamLogos.Length)];
            if (string.IsNullOrEmpty(logoT)) logoT = TeamLogos[Random.Shared.Next(TeamLogos.Length)];
        }

        if (_config.RandomTeamNames)
        {
            if (string.IsNullOrEmpty(nameCT) && !string.IsNullOrEmpty(logoCT))
                nameCT = TeamNames.TryGetValue(logoCT, out var n1) ? n1 : "Unknown";
            if (string.IsNullOrEmpty(nameT) && !string.IsNullOrEmpty(logoT))
                nameT = TeamNames.TryGetValue(logoT, out var n2) ? n2 : "Unknown";
        }

        if (!string.IsNullOrEmpty(nameCT)) Core.Engine.ExecuteCommand($"mp_teamname_1 \"{nameCT}\"");
        if (!string.IsNullOrEmpty(nameT)) Core.Engine.ExecuteCommand($"mp_teamname_2 \"{nameT}\"");
        if (!string.IsNullOrEmpty(logoCT)) Core.Engine.ExecuteCommand($"mp_teamlogo_1 {logoCT}");
        if (!string.IsNullOrEmpty(logoT)) Core.Engine.ExecuteCommand($"mp_teamlogo_2 {logoT}");

        Core.Logger.LogInformation("[TeamIdentityManager] Applied: CT = {CtName} ({CtLogo}), T = {TName} ({TLogo})", nameCT, logoCT, nameT, logoT);
    }

    private void LogDebug(string message, params object?[] args)
    {
        if (_config.DebugMode) Core.Logger.LogDebug("[TeamIdentityManager] " + message, args);
    }
}