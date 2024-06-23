namespace FunctionalTest;

[CollectionDefinition(nameof(FunctionalTestCollection))]
public class FunctionalTestCollection : ICollectionFixture<SharedFixture>
{
    // This class has no code, and is never created. Its purpose is simply to be the place
    // to apply [CollectionDefinition] and all the ICollectionFixture<> interfaces.
}