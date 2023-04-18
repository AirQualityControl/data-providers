using System.Runtime.Serialization;

namespace AirSnitch.DataProviders.Lun.OSM;

[DataContract]
public class BaseGeocodingResponse
{
	[DataMember(Name = "place_id")]
	public string PlaceId { get; set; }

	[DataMember(Name = "licence")]
	public string Licence { get; set; }

	[DataMember(Name= "osm_type")]
	public string OsmType { get; set; }

	[DataMember(Name = "osm_id")]
	public string OsmId { get; set; }

	[DataMember(Name = "lat")]
	public string Lat { get; set; }

	[DataMember(Name = "lon")]
	public string Lng { get; set; }

	[DataMember(Name = "boundingbox")]
	public double[] BoundingBox { get; set; }

	[DataMember(Name = "display_name")]
	public string DisplayName { get; set; }

}

