using Microsoft.AspNetCore.Mvc;
using QuanLyHangHoaEsteel_BackEnd.Models;
using QuanLyHangHoaEsteel_BackEnd.Repository;

namespace QuanLyHangHoaEsteel_BackEnd.Controllers
{
    [ApiController]
    [Route("api/report")]
    public class ReportController : Controller
    {
        private readonly IReportRepository reportRepository;

        public ReportController(IReportRepository reportRepository)
        {
            this.reportRepository = reportRepository;
        }
        [HttpGet("product")]
        public IActionResult ReportProduct()
        {
            var rs = reportRepository.ExportExcelProd();
            return Ok(new
            {
                response_code = 200,
                response_data = rs,
                name = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_BaoCaoHangHoa.xlsx"
            });
        }
        [HttpPost("in-out-log")]
        public IActionResult ReportLog(ProductLogfilter filter)
        {
            var rs = reportRepository.ExportExcelInOut(filter);
            return Ok(new
            {
                response_code = 200,
                response_data = rs,
                name = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year + "_BaoCaoNhapXuat.xlsx"
            });
        }
    }
}
