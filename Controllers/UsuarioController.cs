using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Biblioteca.Models;
using System.Linq;
using System.Collections.Generic;
using System;




namespace Biblioteca.Controllers
{
    public class UsuarioController : Controller
    {
        /*
        public IActionResult ListaDeUsuarios() {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);

            return View(new UsuarioService().Listar());
        }
        */

        public IActionResult Cadastro() {
            Autenticacao.CheckLogin(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario novoUsuario) {
            UsuarioService usuarioService = new UsuarioService();

            if(novoUsuario.Id == 0) {
                if(usuarioService.Inserir(novoUsuario)){
                    return RedirectToAction("Listagem");
                } else {
                    ViewData["Mensagem"] = "Login já existe, favor escolha outro";
                    return View(novoUsuario);
                }
            } else {
                if(usuarioService.Atualizar(novoUsuario)){
                    return RedirectToAction("Listagem");
                } else {
                    ViewData["Mensagem"] = "Login já existe, favor escolha outro";
                    return View("Edicao", novoUsuario);
                }
            }
        }

        public IActionResult Listagem(string tipoFiltro, string filtro, int p=1) {
            Autenticacao.CheckLogin(this);
            Filtragem objFiltro = null;
            if(!string.IsNullOrEmpty(filtro)) {
                objFiltro = new Filtragem();
                objFiltro.Filtro = filtro;
                objFiltro.TipoFiltro = tipoFiltro;

            }
            int quantidadePorPagina = 5;
            UsuarioService usuarioService = new UsuarioService();
            int totalDeRegistros = usuarioService.NumeroDeUsuarios();
            ICollection<Usuario> lista = usuarioService.ListarTodos(p, quantidadePorPagina, objFiltro);
            ViewData["NroPaginas"] = (int) Math.Ceiling((double) totalDeRegistros/quantidadePorPagina);
            return View(lista);

        }

        public IActionResult Edicao(int id) {
            Autenticacao.CheckLogin(this);
            UsuarioService usuarioService = new UsuarioService();
            Usuario usuario = usuarioService.ObterPorId(id);
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Exclui(int id) {
            Autenticacao.CheckLogin(this);
            UsuarioService usuarioService = new UsuarioService();
            usuarioService.Excluir(id);
            return RedirectToAction("Listagem");
        }
/*
        public IActionResult RegistrarUsuarios(int id) {
            
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View();

        }

        public IActionResult RegistrarUsuarios(Usuario novoUser) {
            
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            
            novoUser.Senha = Criptografo.TextoCriptografado(novoUser.Senha);

            UsuarioService us = new UsuarioService();
            us.incluirUsuario(novoUser);

            return RedirectToAction("cadastroRealizado");

        }

        public IActionResult excluirUsuario(int id) {

            return View(new UsuarioService().Listar(id));

        }

        [HttpPost]
        public IActionResult ExcluirUsuario(string decisao, int id) {

            if(decisao == "EXCUIR") {
                ViewData["Mensagem"] = "Exclusão de Usuário " + new UsuarioService().Listar(id).Nome + "realizada com sucesso";
                new UsuarioService().excluirUsuario(id);
                return View("ListaDeUsuarios", new UsuarioService().Listar());
            } else {
                ViewData["Mensagem"] = "Exclusão de Cancelada";
                return View("ListaDeUsuarios", new UsuarioService().Listar());
            }

        }

        public IActionResult cadastroRealizado() {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaSeUsuarioEAdmin(this);
            return View();
        }

        public IActionResult NeedAdmin() {
            Autenticacao.CheckLogin(this);
            return View();
        }

        public IActionResult Sair() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        */

    }
}