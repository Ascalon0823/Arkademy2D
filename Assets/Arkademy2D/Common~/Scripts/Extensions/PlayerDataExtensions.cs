using System;

namespace Arkademy2D.Common.Extensions
{
    public static class PlayerDataExtensions
    {
        public static bool Valid(this PlayerData playerData)
        {
            return playerData is not null && playerData.Guid != Guid.Empty;
        }
    }
}