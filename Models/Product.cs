using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductAPI.Models;

public class Product
{
	[BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
	public string? Id{ get; set; }

	public required string Code{ get; set; }

	[BsonElement("Name")]
	public required string Name{ get; set; }

	public required int PotSize{ get; set; }

	public required int PlantHeight{ get; set; }

	public string? Color{ get; set; }

	public required string ProductCategory{ get; set; }

	public Dictionary<string, string[]>? validate()
	{
		List<string> validationErrors = new List<string>();

		if (String.IsNullOrEmpty(Code)) {
			validationErrors.Add("Code cannot be empty");
		} else
		if (Code.Length > 13) {
			validationErrors.Add("Code cannot be longer than 13 characters");
		}

		if (String.IsNullOrEmpty(Name)) {
			validationErrors.Add("Name cannot be empty");
		} else
		if (Name.Length > 50) {
			validationErrors.Add("Name cannot be longer than 50 characters");
		}

		if (PotSize == 0) {
			validationErrors.Add("Pot size cannot be zero");
		}

		if (PlantHeight == 0) {
			validationErrors.Add("Plant height cannot be zero");
		}

		if (String.IsNullOrEmpty(ProductCategory)) {
			validationErrors.Add("Product category cannot be empty");
		}

		return validationErrors.IsNullOrEmpty()
			? null
			: new Dictionary<string, string[]>{
            	{ "errors", validationErrors.ToArray() }
        	};
	}

}
