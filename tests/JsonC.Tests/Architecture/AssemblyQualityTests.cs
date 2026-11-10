using System.Reflection;
using System.Text.RegularExpressions;
using Bennewitz.Ninja.AssemblyQuality;
using Bennewitz.Ninja.AssemblyQuality.Rules;
using Bennewitz.Ninja.JsonC;

namespace JsonC.Tests.Architecture;

/// <summary>
/// AssemblyQuality's rules over the library's compiled assembly, read the way it ships: the shape a
/// consumer binds against.
/// </summary>
/// <remarks>
/// Each rule is held to AssemblyQuality's checks: it skipped nothing, it found nothing, and, where
/// the library gives it a candidate, it inspected something. Findings alone cannot fail a rule that
/// never looked. BNAQ1001 counts only public methods that take a token, which this library has none
/// of. BNAQ1002 finds two members, both deliberate, and pins them. BNAQ1003, the layering rule, is
/// left out: it needs a table of which project may reference what, and a one-project library has no
/// layers. Add it with the second project.
/// </remarks>
public sealed class AssemblyQualityTests
{
    private static readonly Assembly Shipped = typeof(JsoncDocument).Assembly;

    /// <summary>
    /// What BNAQ1002 finds, and all it may find: the two editor methods that take the value to write
    /// as a <c>JsonNode</c>.
    /// </summary>
    private static readonly string[] DeliberateSurface =
    [
        "Bennewitz.Ninja.JsonC.JsoncEditor.SetValue",
        "Bennewitz.Ninja.JsonC.JsoncEditor.SetValueEdits",
    ];

    /// <summary>
    /// BNAQ1005: every grant compiled into the library names an assembly the repository's two grant
    /// files allow: the generated list of its own assemblies, and the hand-written External file's
    /// names.
    /// </summary>
    /// <remarks>
    /// Read from the compiled assembly, so a grant from anywhere else (a stray attribute, a project
    /// item, a generator) fails here even when the conventions check cannot see it. BNAQ1005 loads
    /// nothing, so it never fills Skipped, and that assertion is left out.
    /// </remarks>
    [Fact]
    public void BNAQ1005_every_grant_names_an_allowed_assembly()
    {
        string[] allowed =
        [
            .. GrantedNames("AssemblyInfo.InternalsVisibleTo.cs"),
            .. GrantedNames("AssemblyInfo.InternalsVisibleTo.External.cs"),
        ];

        AssemblyRuleResult result = new FriendGrantRule(allowed).Analyze(AssemblyScanContext.Of(Shipped));

        Assert.True(result.Inspected > 0, "BNAQ1005 inspected no grant, so it proved nothing.");
        Assert.True(result.Findings.Count == 0, string.Join("\n", result.Findings));
    }

    /// <summary>BNAQ1001: no public method takes a <c>CancellationToken</c> with a default.</summary>
    [Fact]
    public void BNAQ1001_no_public_method_takes_a_defaulted_cancellation_token()
    {
        AssemblyRuleResult result = new CancellationTokenRule().Analyze(AssemblyScanContext.Of(Shipped));

        Assert.True(result.Skipped.Count == 0, string.Join("\n", result.Skipped));
        Assert.True(result.Findings.Count == 0, string.Join("\n", result.Findings));
    }

    /// <summary>
    /// BNAQ1002: the only serializer DOM type in the public surface is the <c>JsonNode</c> the editor
    /// takes as the value to write, in <c>JsoncEditor.SetValue</c> and <c>SetValueEdits</c>.
    /// </summary>
    /// <remarks>
    /// ⭐ Deliberate, and pinned rather than excluded. JsonC edits JSON, and a value arrives as
    /// System.Text.Json's DOM, which is what ClaudeForge's <c>AgentForge.Core</c> passes. The rule's
    /// default set names <c>System.Text.Json.Nodes</c>, so it inspects JsonC's surface and finds
    /// exactly those two methods. Any other public member that takes or returns a DOM type fails
    /// here, and so does either of the two changing shape: a public API change, which ClaudeForge
    /// hears about first (AGENTS.md).
    /// </remarks>
    [Fact]
    public void BNAQ1002_only_the_editor_s_JsonNode_parameters_appear_in_the_public_surface()
    {
        AssemblyRuleResult result = new SurfaceLeakRule().Analyze(AssemblyScanContext.Of(Shipped));

        Assert.True(result.Inspected > 0, "BNAQ1002 inspected nothing, so it proved nothing.");
        Assert.True(result.Skipped.Count == 0, string.Join("\n", result.Skipped));
        Assert.Equal(
            DeliberateSurface.Order(StringComparer.Ordinal),
            result.Findings.Select(finding => finding.Subject).Order(StringComparer.Ordinal));
    }

    /// <summary>
    /// BNAQ1004: no namespace segment shadows a referenced root. A segment named <c>System</c> inside
    /// this assembly would make <c>System.IO</c> resolve to it in every consumer, failing with CS0234.
    /// </summary>
    [Fact]
    public void BNAQ1004_no_namespace_segment_shadows_a_referenced_root()
    {
        AssemblyRuleResult result = new NamespaceShadowRule().Analyze(AssemblyScanContext.Of(Shipped));

        Assert.True(result.Inspected > 0, "BNAQ1004 inspected nothing, so it proved nothing.");
        Assert.True(result.Skipped.Count == 0, string.Join("\n", result.Skipped));
        Assert.True(result.Findings.Count == 0, string.Join("\n", result.Findings));
    }

    /// <summary>The names a grant file grants to, comment lines left out.</summary>
    private static IEnumerable<string> GrantedNames(string file) =>
        File.ReadLines(Path.Combine(RepoRoot(), file))
            .Where(line => !line.TrimStart().StartsWith("//", StringComparison.Ordinal))
            .SelectMany(line => Regex.Matches(line, "InternalsVisibleTo\\(\"([^\"]+)\"\\)").Select(m => m.Groups[1].Value));

    private static string RepoRoot()
    {
        DirectoryInfo? directory = new(AppContext.BaseDirectory);

        while (directory is not null && !Directory.EnumerateFiles(directory.FullName, "*.slnx").Any())
        {
            directory = directory.Parent;
        }

        Assert.NotNull(directory);
        return directory.FullName;
    }
}
