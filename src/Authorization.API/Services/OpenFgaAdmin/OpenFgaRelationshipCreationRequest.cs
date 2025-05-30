namespace Authorization.API.Services.OpenFgaAdmin;

public sealed record TupleKeyModel(string user, string relation, string @object)
{

}

public sealed record OpenFgaWrite(TupleKeyModel[] tuple_keys)
{

}

public sealed record OpenFgaRelationshipCreationRequest(OpenFgaWrite writes)
{
}