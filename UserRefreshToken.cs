using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aMongoLibrary;

[BsonCollection("audit")]
public class UserRefreshToken
{
    public ObjectId Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? RevokeDate { get; set; }
    public string RevokeIP { get; set; }
    public string RevokeReason { get; set; }
}
