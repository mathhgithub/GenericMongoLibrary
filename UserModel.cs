using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Entities;
using System.Text.Json.Serialization;

namespace aMongoLibrary;

[BsonCollection("users")]
public class UserModel //:Document
{
    public ObjectId Id { get; set; }

    // nodig voor registratie -----------------------------

    public string Nickname { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string BillingInfo { get; set; }

    // ----------------------------------------------------

    public UserType UserType { get; set; }

    public DateTime AccCreateDay { get; set; }

}

public enum UserType
{
    admin,
    subscriber
}
