namespace MiNET.Net;

public class EducationUriResource
{
	public EducationUriResource()
	{
	}

	public EducationUriResource(string buttonName, string linkUri)
	{
		ButtonName = buttonName;
		LinkUri = linkUri;
	}

	public string ButtonName { get; set; }
	public string LinkUri { get; set; }
}