using System.ComponentModel.DataAnnotations;

public class AddAttractionRequest
{
    [Required]
    public string Name { get; set; }

    [Required]
    [MaxLength(250)]
    public string ShortDescription { get; set; }

    [Required]
    public string LongDescription { get; set; }

    [Required]
    [Range(1, 1440)]
    public int DurationMinutes { get; set; }

    [Required]
    public string AddressOne { get; set; }

    public string AddressTwo { get; set; }

    [Required]
    public int IdCountry { get; set; }

    public List<ImageRequest> Images { get; set; } = new();
    public List<InfoRequest> Infos { get; set; } = new();
}

public class ImageRequest
{
    public string PictureRef { get; set; }
    public bool IsMain { get; set; }
}

public class InfoRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int IdAttractionInfoType { get; set; }
}
