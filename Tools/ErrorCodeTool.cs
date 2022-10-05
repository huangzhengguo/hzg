using Hzg.Const;

namespace Hzg.Tool;

public static class ErrorCodeTool
{
    /// <summary>
    /// 根据错误码获取错误信息
    /// </summary>
    /// <param name="errorCode"></param>
    /// <returns></returns>
    public static string GetErrorMessage(ErrorCode errorCode)
    {
        string message = null;
        switch (errorCode)
        {
            case ErrorCode.Success:
                message ="Success!";
                break;
            case ErrorCode.Create_Success:
                message = "创建成功";
                break;
            case ErrorCode.Update_Success:
                message = "修改成功";
                break;
            case ErrorCode.Confirm_Success:
                message = "确认成功";
                break;
            case ErrorCode.Close_Success:
                message = "关闭成功";
                break;
            case ErrorCode.Registered_Successfully:
                message = "Registered successfully!";
                break;
            case ErrorCode.Delete_Group_Success:
                message = "Delete group success!";
                break;
            case ErrorCode.Delete_Success:
                message = "Delete success!";
                break;

            case ErrorCode.Create_Failed:
                message = "创建失败";
                break;
            case ErrorCode.Update_Failed:
                message = "修改失败";
                break;
            case ErrorCode.Confirm_Failed:
                message = "确认失败";
                break;
            case ErrorCode.Close_Failed:
                message = "关闭失败";
                break;
            case ErrorCode.NotExist:
                message = "Not exist!";
                break;
            case ErrorCode.Device_Not_Exist:
                message = "The device not exist!";
                break;
            case ErrorCode.Illegal_Token:
                message = "Illegal token!";
                break;
            case ErrorCode.Device_Has_Exist:
                message = "The device has exist!";
                break;
            case ErrorCode.User_Has_Exist:
                message = "The user has exist!";
                break;
            case ErrorCode.VerifyCode_Expire:
                message = "Verify code expire!";
                break;
            case ErrorCode.VerifyCode_Incorrect:
                message = "Verify code incorrect!";
                break;
            case ErrorCode.User_Not_Exist:
                message = "The user not exist!";
                break;
            case ErrorCode.Password_Not_Correct:
                message = "Password not correct!";
                break;
            case ErrorCode.Account_Or_Password_Not_Correct:
                message = "Account Or Password not correct!";
                break;
            case ErrorCode.Group_Has_Exist:
                message = "Group has exist!";
                break;
            case ErrorCode.Group_Not_Exist:
                message = "Group not exist!";
                break;
            case ErrorCode.Group_Has_User:
                message = "Group has user!";
                break;  
            case ErrorCode.Menu_Has_Exist:
                message = "Menu has exist!";
                break; 
            case ErrorCode.Delete_Failed:
                message = "Delete failed!";
                break;
            case ErrorCode.Failed:
                message = "Failed!";
                break;
            case ErrorCode.Role_Has_Exist:
                message = "Role has exist!";
                break;  
            case ErrorCode.Role_Not_Exist:
                message = "Role not exist!";
                break;
            case ErrorCode.Modify_Password_Failed:
                message = "Modify Password Failed!";
                break;  
            case ErrorCode.Reset_Password_Failed:
                message = "Modify Password Failed!";
                break;   
            case ErrorCode.Not_Administrator_Edit_Group_Info:
                message = "Only the group administrator can modify the group information!";
                break;  
            case ErrorCode.Can_Not_Delete_Default_Group:
                message = "Could not delete default group!";
                break;    
            case ErrorCode.Delete_Group_Failed:
                message = "Failed to delete the group!";
                break;   
            case ErrorCode.User_Has_Not_Create_A_Group:
                message = "The user has not created a group!";
                break;    
            case ErrorCode.Not_Administrator_Add_Device_To_Group:
                message = "Only administrator is allowed to add devices to the group!";
                break;
            case ErrorCode.No_Device_In_Group:
                message = "There are no devices in this group!";
                break; 
            case ErrorCode.User_Not_In_Group:
                message = "The user is not in the group!";
                break; 
            case ErrorCode.User_Quit_From_Group_Failed:
                message = "Failed to quit from the group!";
                break;
            case ErrorCode.Product_Has_Exist:
                message = "Product has exist!";
                break; 
            case ErrorCode.Product_Not_Exist:
                message = "Product not exist!";
                break; 
            case ErrorCode.Corp_Not_Exist:
                message = "Corp not exist!";
                break;
            case ErrorCode.Admin_Not_Allow_Leave_Group:
                message = "The administrator is not allowed to leave the group!";
                break;
            case ErrorCode.Admin_Only_Allow_Add_User_To_Group:
                message = "The administrator is not allowed add user to the group!";
                break;
            case ErrorCode.Admin_Only_Allow_Delete_User_From_Group:
                message = "Only administrator allowed to delete user from group!";
                break;
            default:
                message = null;
                break;
        }

        return message;
    }
}