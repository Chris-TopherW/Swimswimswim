
public class NotationTime
{
    public int bar;
    public int quarter;
    public int tick;

    public NotationTime(int bar, int quarter, int tick)
    {
        this.bar = bar;
        this.quarter = quarter;
        this.tick = tick;
    }

    public NotationTime(NotationTime other)
    {
        this.bar = other.bar;
        this.quarter = other.quarter;
        this.tick = other.tick;
    }

    public bool isLooping()
    {
        return (bar != 0 || quarter != 0 || tick != 0);
    }

    public void Add(NotationTime other)
    {
        tick += other.tick;
        if (tick >= 8)
        {
            tick -= 8;
            quarter++;
        }
        quarter += other.quarter;
        if (quarter >= 4)
        {
            quarter -= 4;
            bar++;
        }
        bar += other.bar;

    }

    public void AddTick() {
        tick++;
        if (tick >= 8) {
            tick -= 8;
            quarter++;
        }
        if (quarter >= 4) {
            quarter -= 4;
            bar++;
        }
    }

	public void AddTime(int bar, int quarter, int tick) {
		for(int i = 0; i < tick; i++) 
		{
			this.tick++;
			if (this.tick >= 8) {
				this.tick -= 8;
				this.quarter++;
			}
		}
		for(int i = 0; i < quarter; i++) 
		{
			this.quarter++;
			if (this.quarter >= 8) {
				this.quarter -= 8;
				this.bar++;
			}
		}
		this.bar += bar;
	}

	public void AddTime(NotationTime notationTime) {
		for(int i = 0; i < notationTime.tick; i++) 
		{
			tick++;
			if (tick >= 8) {
				tick -= 8;
				quarter++;
			}
		}
		for(int i = 0; i < notationTime.quarter; i++) 
		{
			quarter++;
			if (quarter >= 8) {
				quarter -= 8;
				bar++;
			}
		}
		bar += notationTime.bar;
	}

    protected bool Equals(NotationTime other)
    {
        return bar == other.bar && quarter == other.quarter && tick == other.tick;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((NotationTime) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = bar;
            hashCode = (hashCode*397) ^ quarter;
            hashCode = (hashCode*397) ^ tick;
            return hashCode;
        }
    }

    public int TimeAsTicks()
    {
        return (bar * 32) + (quarter * 8) + tick;
    }
}