using System;
using System.ComponentModel.DataAnnotations;

namespace Hzg.Consts;

/// <summary>
/// 错误码常量：前三个数字错误分类，后三个数字标识具体错误
/// </summary>
public enum ErrorCode : int
{
    // 成功错误码 20000 - 20099
    Success = 0,
    Query_Success = 20001,
    Create_Success = 20002,
    Update_Success = 20003,
    Confirm_Success = 20004,
    Close_Success = 20005,
    // 注册成功
    Registered_Successfully = 20006,
    // 删除分组成功
    Delete_Group_Success = 20007,
    // 删除成功
    Delete_Success = 20008,

    // 失败错误码 20100 - 20099
    Failed = 21000,
    // 查询失败
    Query_Failed = 21001,
    // 创建失败
    Create_Failed = 21002,
    // 更新失败
    Update_Failed = 21003,
    // 确认失败
    Confirm_Failed = 21004,
    // 关闭失败
    Close_Failed = 21005,
    // 设备不存在
    Device_Not_Exist = 21006,
    // 无效 token
    Illegal_Token = 21007,
    // 设备已存在
    Device_Has_Exist = 21008,
    // 用户已存在
    User_Has_Exist = 21009,
    // 验证码过期
    VerifyCode_Expire = 21010,
    // 不正确
    VerifyCode_Incorrect = 21011,
    // 用户不存在
    User_Not_Exist = 21012,
    // 密码不正确
    Password_Not_Correct = 21013,
    // 账号或密码不正确
    Account_Or_Password_Not_Correct = 21014,
    // 分组已存在
    Group_Has_Exist = 21015,
    // 分组不存在
    Group_Not_Exist = 21016,
    // 分组存在用户，无法删除
    Group_Has_User = 21017,
    // 菜单已存在
    Menu_Has_Exist = 21018,
    // 删除失败
    Delete_Failed = 21019,
    // 角色已存在
    Role_Has_Exist = 21020,
    // 角色不存在
    Role_Not_Exist = 21021,
    // 修改密码失败
    Modify_Password_Failed = 21022,
    // 重置密码失败
    Reset_Password_Failed = 21023,
    // 只有管理员才能修改分组信息
    Not_Administrator_Edit_Group_Info = 21024,
    // 不能删除默认分组
    Can_Not_Delete_Default_Group = 21025,
    // 删除分组出错
    Delete_Group_Failed = 21026,
    // 用户还未创建分组
    User_Has_Not_Create_A_Group = 21027,
    // 非管理员不能添加设备到分组
    Not_Administrator_Add_Device_To_Group = 21028,
    // 非管理员不能添加设备到分组
    No_Device_In_Group = 21029,
    // 用户不在分组中
    User_Not_In_Group = 21030,
    // 用户退出分组失败
    User_Quit_From_Group_Failed = 21031,
    // 产品已存在
    Product_Has_Exist = 21032,
    // 产品不存在
    Product_Not_Exist = 21033,
    // 公司不存在
    Brand_Not_Exist = 21034,
    // 管理禁止退出分组
    Admin_Not_Allow_Leave_Group = 21035,
    // 禁止普通用户添加
    Admin_Only_Allow_Add_User_To_Group = 21036,
    // 只能管理员删除用户
    Admin_Only_Allow_Delete_User_From_Group = 21037,
    // 子产品不存在
    Sub_Product_Not_Exist = 21038,
    // 请选择产品
    Please_Select_Product = 21039,
    // 一次最多绑定的设备
    Max_Bind_Count = 21040,
    // 没有权限
    Has_No_Permission = 21041,
    // 产品已存在
    Has_Exist = 21042,
    // 邮箱格式不正确
    Email_Format_Error = 21043,
    // 验证码发送频繁模板
    Verify_Code_Too_Frequency_Tepmlete = 21044,
    // 已关闭
    Has_Closed = 21045,
    // 不存在
    NotExist = 20046,
    // 内容不能为空
    Content_Can_Not_Empty = 20047,
    // 时间过长
    Date_Range_Too_Long = 20048,
    // 密码不一致
    New_Old_Password_Not_Same = 21049
}