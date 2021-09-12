﻿using Room.Core.Models;

namespace Room.Core.Abstract
{
    public interface IArenaFactory
    {
        Arena Create(int level);
    }
}