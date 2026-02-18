namespace JournalChallenge.ArchitectureTests;

using System.Reflection;

using JournalChallenge.Application;
using JournalChallenge.Domain;
using JournalChallenge.Infrastructure;
using JournalChallenge.Presentation;
using JournalChallenge.Tests.Core.Abstractions;

using NUnit.Framework;

[TestFixture]
public class LayerTests: BaseLayerTests
{
    protected override Assembly DomainAssembly { get; init; } = typeof(DomainAbstraction).Assembly;

    protected override Assembly PresentationAssembly { get; init; } = typeof(PresentationAbstraction).Assembly;

    protected override Assembly InfrastructureAssembly { get; init; } = typeof(InfrastructureAbstraction).Assembly;

    protected override Assembly ApplicationAssembly { get; init; } = typeof(ApplicationAbstraction).Assembly;
}