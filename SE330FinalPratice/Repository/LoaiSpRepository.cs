using BaiTap4.Models;

namespace BaiTap4.Repository
{
	public class LoaiSpRepository : ILoaiSpRepository
	{
		private readonly QlbanVaLiContext _context;
		public LoaiSpRepository(QlbanVaLiContext context)
		{
			_context = context;
		}
		public TLoaiSp Add(TLoaiSp loaiSp)
		{
			_context.TLoaiSps.Add(loaiSp);
			_context.SaveChanges();
			return loaiSp;
		}

		public TLoaiSp Delete(string maloaiSp)
		{
			_context.TLoaiSps.Remove(GetLoaiSp(maloaiSp));
			_context.SaveChanges();
			return GetLoaiSp(maloaiSp);
		}

		public IEnumerable<TLoaiSp> GetAllLoaiSp()
		{
			return _context.TLoaiSps;
		}

		public TLoaiSp GetLoaiSp(string maloaiSp)
		{
			return _context.TLoaiSps.Find(maloaiSp);
		}

		public TLoaiSp Update(TLoaiSp loaiSp)
		{
			_context.TLoaiSps.Update(loaiSp);
			_context.SaveChanges();
			return loaiSp;
		}
	}
}
