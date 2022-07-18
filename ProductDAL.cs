using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace aMongoLibrary;

[BsonCollection("products")]
public class ProductDAL //:Document
{
    public ObjectId Id { get; set; }

    [Required]
    public string SKU { get; set; }

    [Required]
    public string Title { get; set; }

    
    public string BigCategory { get; set; }

    public double CurrentPrice { get; set; }

    public DateTime AuctionDate { get; set; }

    public string ImgUrl { get; set; }

    public string ImgUrlQuery { get; set; }

}