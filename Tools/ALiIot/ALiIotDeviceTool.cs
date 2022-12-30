using AlibabaCloud.SDK.Iot20180120;
using AlibabaCloud.SDK.Iot20180120.Models;
using System.ComponentModel.DataAnnotations;
// using Inledco.Iot.Dto;

namespace Hzg.Tool;

public class SubscribeDto
{

    public static long serialVersionUID = 1L;

    /// <summary>
    /// 产品ID
    /// </summary>
    /// <value></value>
    [Required]
    public String ProductKey { get; set; }

    /// <summary>
    /// 设备ID
    /// </summary>
    /// <value></value>
    [Required]
    public String DeviceName { get; set; }

    /// <summary>
    /// 设备mac地址
    /// </summary>
    /// <value></value>
    [Required]
    public String Mac { get; set; }
}

public class ALiIotDeviceTool
{
    /// <summary>
    /// 阿里云物联网配置
    /// </summary>
    private readonly HzgAliIotConfig hzgAliIotConfig;

    public ALiIotDeviceTool(HzgAliIotConfig config)
    {
        this.hzgAliIotConfig = config;
    }

    /// <summary>
    /// 初始化阿里云物联网客户端
    /// </summary>
    /// <returns></returns>
    private Client Common()
    {
        if (hzgAliIotConfig == null)
        {
            throw new ArgumentNullException("阿里云物联网配置错误");
        }

        return new Client(hzgAliIotConfig.Config);
    }

    /// <summary>
    /// 阿里云物联网注册设备
    /// </summary>
    /// <param name="deviceInfo"></param>
    /// <returns></returns>
    public async Task<RegisterDeviceResponse> CreateDevice(SubscribeDto deviceInfo)
    {
        var client = Common();

        var request = new RegisterDeviceRequest();

        request.ProductKey = deviceInfo.ProductKey;
        request.DeviceName = deviceInfo.DeviceName;
        RegisterDeviceResponse response = null;
        
        try
        {
            response = await client.RegisterDeviceAsync(request);
        }
        catch(Exception e)
        {
            throw new Exception("CreateDevice 注册设备失败: " + e.Message);
        }
        

        return response;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="productKey"></param>
    /// <param name="deviceName"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<DeleteDevicePropResponse> DeleteDeviceProp(String productKey, String deviceName, String userName)
    {
        DeleteDevicePropRequest devicePropRequest = new DeleteDevicePropRequest();

        devicePropRequest.ProductKey = productKey;
        devicePropRequest.DeviceName = deviceName;
        devicePropRequest.PropKey = userName;

        DeleteDevicePropResponse deleteDevicePropResponse = null;

        try
        {
            var client = Common();

            deleteDevicePropResponse = await client.DeleteDevicePropAsync(devicePropRequest);
        }
        catch(Exception e)
        {
            throw new Exception("删除标签出错: " + e.Message);
        }

        return deleteDevicePropResponse;
    }

    /// <summary>
    /// 添加设备标签
    /// </summary>
    /// <param name="productKey"></param>
    /// <param name="deviceName"></param>
    /// <param name="userId"></param>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<SaveDevicePropResponse> SaveDeviceProp(String productKey,String deviceName, String userId, String userName)
    {
        var client = Common();

        SaveDevicePropRequest saveDevicePropRequest = new SaveDevicePropRequest();

        saveDevicePropRequest.ProductKey = productKey;
        saveDevicePropRequest.DeviceName = deviceName;

        Dictionary<string, string> newHashMap = new Dictionary<string, string>();

        newHashMap[userName] = userId;

        saveDevicePropRequest.Props = newHashMap.ToString();

        SaveDevicePropResponse response = null;
        try
        {
            response = await client.SaveDevicePropAsync(saveDevicePropRequest);
        }
        catch (Exception e)
        {
            throw new Exception("删除标签出错: " + e.Message);
        }

        return response;
    }

    /// <summary>
    /// 查询设备详情
    /// </summary>
    /// <param name="iotId"></param>
    /// <param name="productKey"></param>
    /// <param name="deviceName"></param>
    /// <returns></returns>
    public async Task<QueryDeviceDetailResponse> QueryDeviceDetail(String iotId,String productKey,String deviceName)
    {
        var client = Common();
        QueryDeviceDetailRequest queryDeviceDetailRequest = new QueryDeviceDetailRequest();
        if (string.IsNullOrEmpty(iotId) == false)
        {
            queryDeviceDetailRequest.IotId = iotId;
        }
        else
        {
            queryDeviceDetailRequest.DeviceName = deviceName;
            queryDeviceDetailRequest.ProductKey = productKey;
        }

        QueryDeviceDetailResponse response = await client.QueryDeviceDetailAsync(queryDeviceDetailRequest);

        return response;
    }

    public async Task<BatchGetDeviceStateResponse> BatchGetDeviceState(List<String> iotids)
    {
        var client = Common();
        BatchGetDeviceStateRequest request = new BatchGetDeviceStateRequest();

        request.IotId = iotids;

        BatchGetDeviceStateResponse response = await client.BatchGetDeviceStateAsync(request);

        return response;
    }
}