using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Registrar_Funcionario.Models;
using FluentDateTime;

namespace Registrar_Funcionario.Controllers
{
    public class FuncionariosController : Controller
    {
        private Context db = new Context();

        // GET: Funcionarios
        public async Task<ActionResult> Index()
        {
            return View(await db.Funcionarios.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Index(FormCollection fc)
        {
            string nome = String.IsNullOrEmpty(fc["nome"]) ? "" : fc["nome"];
            string cpf = String.IsNullOrEmpty(fc["cpf"]) ? "" : fc["cpf"];
            IQueryable<Funcionario> buscar = from f in db.Funcionarios
                                             where f.Nome.Contains(nome)
                                             where f.CPF.Contains(cpf)
                                             select f;

            if (!String.IsNullOrEmpty(nome) && !String.IsNullOrEmpty(cpf))
                ViewBag.Message = $"Resultado da pesquisa por Nome: {nome} e CPF: {cpf}";
            else if (!String.IsNullOrEmpty(nome))
                ViewBag.Message = $"Resultado da pesquisa por Nome: {nome}";
            else if (!String.IsNullOrEmpty(cpf))
                ViewBag.Message = $"Resultado da pesquisa por CPF: {cpf}";
            else
                ViewBag.Message = "Todos os registros";

            return View(buscar.ToList());
        }

        public async Task<ActionResult> Calcular()
        {
            return View(await db.Funcionarios.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Calcular(FormCollection fc)
        {
            try
            {
                DateTime mes = DateTime.Parse(fc["mes"]).LastDayOfMonth();

                IQueryable<decimal?> gasto = from f in
                                            (from f in db.Funcionarios
                                             where
                                               f.Data_admissao <= mes
                                             select new
                                             {
                                                 f.Salario,
                                                 Dummy = "x"
                                             })
                                             group f by new { f.Dummy } into g
                                             select new
                                             {
                                                 Gasto = (decimal?)g.Sum(p => p.Salario)
                                             }.Gasto;

                ViewBag.Message = $"No mês {mes.ToString("MM/yyyy")} o gasto em salário {(mes <= DateTime.Now ? "foi" : "será")} de R$: {gasto.FirstOrDefault() ?? 0}";

                return View(await db.Funcionarios.ToListAsync());
            }
            catch (Exception)
            {
                ViewBag.Message = "Preencha a data";
                return View(await db.Funcionarios.ToListAsync());
            }
        }

        // GET: Funcionarios/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionario funcionario = await db.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            return View(funcionario);
        }

        // GET: Funcionarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Funcionarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nome,Sexo,PIS,CPF,Salario,Email,Data_admissao")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                db.Funcionarios.Add(funcionario);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(funcionario);
        }

        // GET: Funcionarios/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionario funcionario = await db.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            return View(funcionario);
        }

        // POST: Funcionarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nome,Sexo,PIS,CPF,Salario,Email,Data_admissao")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(funcionario).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(funcionario);
        }

        // GET: Funcionarios/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionario funcionario = await db.Funcionarios.FindAsync(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            return View(funcionario);
        }

        // POST: Funcionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Funcionario funcionario = await db.Funcionarios.FindAsync(id);
            db.Funcionarios.Remove(funcionario);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
