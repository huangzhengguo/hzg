namespace Hzg.Dto;

/// <summary>
/// 
/// </summary>
public class UserEditDto
{

    public static long serialVersionUID = 1L;

    /// <summary>
    /// 头像图片路径
    /// </summary>
    /// <value></value>
    public String Avatar { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    /// <value></value>
    public String Nickname { get; set; }

    /// <summary>
    /// 谷歌token
    /// </summary>
    /// <value></value>
    public String Remark1 { get; set; }

    /// <summary>
    /// 苹果token
    /// </summary>
    /// <value></value>
    public String Remark2 { get; set; }

    /// <summary>
    /// 备注3
    /// </summary>
    /// <value></value>
    public String Remark3 { get; set; }

    /// <summary>
    /// 谷歌推送Token
    /// </summary>
    /// <value></value>
    public String SendFcmToken { get; set; }

    /// <summary>
    /// 苹果推送Token
    /// </summary>
    /// <value></value>
    public String SendIosToken { get; set; }
}
