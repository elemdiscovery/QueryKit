using Marten;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;
using Testcontainers.PostgreSql;
using Xunit;

namespace QueryKit.MartenTests;

public abstract class TestBase : IAsyncLifetime
{
    private readonly IDocumentStore _store;
    private readonly IServiceScope _scope;
    protected IDocumentSession Session;
    private readonly PostgreSqlContainer _dbContainer;

    protected TestBase()
    {
        _dbContainer = new PostgreSqlBuilder().Build();

        var services = new ServiceCollection();

        services.AddMarten(opts =>
        {
            opts.Connection("Host=localhost;Port=5432;Database=querykit_tests;Username=postgres;Password=postgres");
            opts.AutoCreateSchemaObjects = AutoCreate.All;
        });

        var provider = services.BuildServiceProvider();
        _scope = provider.CreateScope();
        _store = provider.GetRequiredService<IDocumentStore>();
        Session = _store.LightweightSession();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();

        var services = new ServiceCollection();
        services.AddMarten(opts =>
        {
            opts.Connection(_dbContainer.GetConnectionString());
            opts.AutoCreateSchemaObjects = AutoCreate.All;
        });

        var provider = services.BuildServiceProvider();
        _scope?.Dispose();
        _store?.Dispose();

        var scope = provider.CreateScope();
        var store = provider.GetRequiredService<IDocumentStore>();
        Session?.Dispose();
        Session = store.LightweightSession();
    }

    public async Task DisposeAsync()
    {
        Session?.Dispose();
        _scope?.Dispose();
        _store?.Dispose();
        await _dbContainer.DisposeAsync();
    }
}