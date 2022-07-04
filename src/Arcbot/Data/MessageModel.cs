using System;

namespace Arcbot.Data;

public class MessageModel
{
    /// <summary>
    /// Model Id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Sender (Member or Friend)
    /// </summary>
    public long Sender { get; set; }
    /// <summary>
    /// _ if Friend
    /// </summary>
    public long Group { get; set; }
    // Message in Json
    public string Content { get; set; }
    /// <summary>
    /// UTC Date Time
    /// </summary>
    public DateTime Time { get; set; }
}