using System.Runtime.Serialization;

namespace AirSnitch.DataProviders.Lun.OSM;

[DataContract]
public class AddressResponse: BaseGeocodingResponse
{
	[DataMember(Name = "address")]
	public Address Address { get; set; }
}

