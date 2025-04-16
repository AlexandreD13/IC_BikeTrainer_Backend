SELECT device.Id AS DeviceId, device.UserId, device.DeviceName, device.MacAddress
FROM UsersTable user
         LEFT JOIN DevicesTable device ON user.Id = device.UserId
WHERE user.Username = 'johndoe';