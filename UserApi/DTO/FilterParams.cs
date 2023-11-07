namespace TestApplication.DTO;

/// <summary>
///     Request model for filtering data based on specified parameters.
/// </summary>
public class FilterParams
{
    /// <summary>
    ///     Gets or sets the minimum value for filtering.
    /// </summary>
    public int Min { get; set; }

    /// <summary>
    ///     Gets or sets the maximum value for filtering.
    /// </summary>
    public int Max { get; set; }

    /// <summary>
    ///     Gets or sets the parameter to use for filtering.
    /// </summary>
    public string Param { get; set; }
}