using System.Runtime.Serialization;

namespace AirSnitch.DataProviders.Lun.OSM;

[DataContract]
public class Address
{
	[DataMember(Name = "house_number")]
	public string HouseNomber { get; set; }
	[DataMember(Name = "road")]
	public string Road { get; set; }
	[DataMember(Name = "suburb")]
	public string Suburb { get; set; }
	[DataMember(Name = "city")]
	public string City { get; set; }
	[DataMember(Name = "neighbourhood")]
	public string Neighbourhood { get; set; }
	[DataMember(Name = "state")]
	public string State { get; set; }
	[DataMember(Name = "postcode")]
	public string PostCode { get; set; }
	[DataMember(Name= "country")]
	public string Country { get; set; }
	[DataMember(Name = "country_code")]
	public string CountryCode { get; set; }

}

