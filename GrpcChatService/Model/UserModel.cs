using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcChatService.Model
{

    /// <summary>
    /// 用戶模型
    /// </summary>
    public class UserModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string AvatarUrl { get; set; } = "";
        public string Role { get; set; } = "";
        public DateTime RegisterDate { get; set; }
        public bool IsOnline { get; set; }
        public bool IsBanned { get; set; }
    }
}