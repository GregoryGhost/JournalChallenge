namespace JournalChallenge.Tests.Core.Abstractions;

using System.Reflection;

using JournalChallenge.Application.Core;
using JournalChallenge.Infrastructure.Core;

using NetArchTest.Rules;

using NUnit.Framework;

using Shouldly;

public abstract class BaseLayerTests
{
    protected abstract Assembly ApplicationAssembly { get; init; }

    protected abstract Assembly DomainAssembly { get; init; }

    protected abstract Assembly InfrastructureAssembly { get; init; }

    protected abstract Assembly PresentationAssembly { get; init; }
    
    private Assembly ApplicationLayerCore { get; init; } = typeof(ApplicationCoreAbstraction).Assembly;

    private  Assembly InfrastructureLayerCore { get; init; } = typeof(InfrastructureCoreAbstraction).Assembly;

    [Test]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        var result = Types.InAssembly(ApplicationAssembly)
                          .Should()
                          .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                          .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Test]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        var result = Types.InAssembly(ApplicationAssembly)
                          .Should()
                          .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
                          .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Test]
    public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
    {
        var result = Types.InAssembly(DomainAssembly)
                          .Should()
                          .NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
                          .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Test]
    public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
    {
        var result = Types.InAssembly(DomainAssembly)
                          .Should()
                          .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                          .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Test]
    public void DomainLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        var result = Types.InAssembly(DomainAssembly)
                          .Should()
                          .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
                          .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }

    [Test]
    public void InfrastructureLayer_ShouldNotHaveDependencyOn_PresentationLayer()
    {
        var result = Types.InAssembly(InfrastructureAssembly)
                          .Should()
                          .NotHaveDependencyOn(PresentationAssembly.GetName().Name)
                          .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
    
    [Test]
    public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayerCore()
    {
        var result = Types.InAssembly(ApplicationAssembly)
                          .Should()
                          .NotHaveDependencyOn(InfrastructureLayerCore.GetName().Name)
                          .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
    
    [Test]
    public void ApplicationLayerCore_ShouldHaveDependencyOn_InfrastructureLayerCore()
    {
        var result = Types.InAssembly(ApplicationLayerCore)
                          .Should()
                          .NotHaveDependencyOn(InfrastructureLayerCore.GetName().Name)
                          .GetResult();

        result.IsSuccessful.ShouldBeTrue();
    }
}