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
/*
        public IActionResult Excluir() {
            Autenticacao.CheckLogin(this);
            UsuarioService usuarioService = new UsuarioService();
            Usuario usuario = usuarioService.ObterPorId(id);
            return View("Excluir");
        }

        [HttpPost]
        public IActionResult Excluir(int id) {
            Autenticacao.CheckLogin(this);
            UsuarioService usuarioService = new UsuarioService();
            usuarioService.Excluir(id);
            return RedirectToAction("Listagem");
        }
        */

        public IActionResult Excluir(int id)
    {
        UsuarioService usuarioService = new UsuarioService();
        Usuario usuario = usuarioService.ObterPorId(id);

        return View(usuario);
    }

    [HttpPost]
    public IActionResult ConfirmarExclusao(int id)
    {
        UsuarioService usuarioService = new UsuarioService();
        Usuario usuario = usuarioService.ObterPorId(id);

        usuarioService.Excluir(id);

        return RedirectToAction("Listagem"); // Redirecione para a página principal ou outra página desejada
    }

    }
}