namespace Salt.Models;

public record TimelineNamedMarker {
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public uint Start { get; set; }
	public uint Length { get; set; }
}
