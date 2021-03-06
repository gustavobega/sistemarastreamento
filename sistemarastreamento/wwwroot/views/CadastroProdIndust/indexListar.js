﻿var indexListar = {

    block: function (id) {
        var acoes = document.getElementById('acoesoption' + id);
        var opcoes = document.getElementsByClassName('dropdown-content')

        for (i = 0; i < opcoes.length; i++) {
            if (opcoes[i].id != "acoesoption" + id)
            opcoes[i].style.display = 'none'
        }

        if (acoes.style.display == 'none')
            acoes.style.display = 'block';
        else
            acoes.style.display = 'none';
    },
    excluir: function (id) {

        const swalWithBootstrapButtons = Swal.mixin({
            /*customClass: {
                confirmButton: 'buttonSucess',
                cancelButton: 'buttonDanger'
            },*/
            buttonsStyling: true
        })

        swalWithBootstrapButtons.fire({
            title: 'Confirmar Exclusão?',
            text: "Você não poderá reverter isso!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sim',
            cancelButtonText: 'Não',
            reverseButtons: true
        })
        .then((result) => {
            if (result.isConfirmed) {

                var config = {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json; charset=utf-8"
                    },
                    credentials: 'include', //inclui cookies
                };

                fetch("/CadastroProdIndust/Excluir?id=" + id, config)
                    .then(function (dadosJson) {
                        var obj = dadosJson.json(); //deserializando
                        return obj;
                    })
                    .then(function (dadosObj) {
                        if (dadosObj.operacao) {
                            document.getElementById(id).remove()
                        }
                    })
                    .catch(function () {
                        alert("Deu erro.")
                    })

                swalWithBootstrapButtons.fire(
                    'Deletado!',
                    'Produto Deletado.',
                    'success'
                )
            }
            else if (result.dismiss === Swal.DismissReason.cancel) {
                swalWithBootstrapButtons.fire(
                    'Cancelado',
                    'Nada Alterado :)',
                    'error'
                )
            }
        }) 

    },

    alterar: function (id) {
        var config = {
            method: "GET",
            headers: {
                
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/CadastroProdIndust/Alterar?id=" + id, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                alert(dadosObj.prodindust.descricao);

            })
            .catch(function () {
                alert("Deu erro.")
            })
    },

    btnPesquisarOnClick: function (perfil) {
        document.getElementById("iconsearch").disabled = true;
        var produto = encodeURIComponent(document.getElementById("produto").value);
        var tbodyProdutos = document.getElementById("table");

        if (produto.trim().length > 3) {

            tbodyProdutos.innerHTML = `
                                   <div class="table-row table-head">
                                        <div class="table-cell first-cell">
                                            <p>Id</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Código de Referência</p>
                                        </div>
                                        <div class="table-cell table-cell-descricao">
                                            <p>Descrição</p>
                                        </div>
                                        <div class="table-cell last-cell headacoes">
                                            <p>Açoes</p>
                                        </div>
                                    </div>
                                    `
            tbodyProdutos.innerHTML += `
                                    <div class="table-row">
                                        <div class="table-cell first-cell">
                                            <p><img src=\"/img/ajax-loader.gif"\/>carregando...</p>
                                        </div>
                                    </div>
                                    `

            var config = {
                method: "GET",
                headers: {
                    "Content-Type": "application/json; charset=utf-8"
                },
                credentials: 'include', //inclui cookies
            };
            var tipo = $(".default_option").text();

            fetch("/CadastroProdIndust/Pesquisar?produto=" + produto + "&tipo=" + tipo, config)
                .then(function (dadosJson) {
                    var obj = dadosJson.json(); //deserializando
                    return obj;
                })
                .then(function (dadosObj) {

                    if (dadosObj.operacao) {

                        var linhas = ` 
                        <div class="table-row table-head">
                            <div class="table-cell first-cell">
                                <p>Id</p>
                            </div>
                            <div class="table-cell">
                                <p>Código de Referência</p>
                            </div>
                            <div class="table-cell table-cell-descricao">
                                <p>Descrição</p>
                            </div>
                            <div class="table-cell last-cell headacoes">
                                <p>Açoes</p>
                            </div>
                        </div>
                        `;

                        for (var i = 0; i < dadosObj.produtosLimpos.length; i++) {

                            var template =
                                `
                        <div class="table-row" id="${dadosObj.produtosLimpos[i].id}">
                            <div class="table-cell first-cell">
                                <p>${dadosObj.produtosLimpos[i].id}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.produtosLimpos[i].cod_ref}</p>
                            </div>
                            <div class="table-cell table-cell-descricao">
                                <p>${dadosObj.produtosLimpos[i].descricao}</p>
                            </div>
                            <div class="table-cell last-cell acoes">
                                <div class="dropdown" onclick="indexListar.block(${dadosObj.produtosLimpos[i].id})">
                                    <i class="dropbtn fa fa-fw fa-ellipsis-v"></i>
                                    <div class="dropdown-content" id="acoesoption${dadosObj.produtosLimpos[i].id}" style="display: none;">
                                        <a href="/CadastroProdIndust/Editar?id=${dadosObj.produtosLimpos[i].id}"><i class='bx bx-edit'></i> Editar</a>
                                        <a href="javascript:indexListar.excluir(${dadosObj.produtosLimpos[i].id})"><i class='bx bxs-user-x'></i> Excluir</a>
                                  </div>
                                </div>
                            </div>
                        </div>
                        `

                            linhas += template;
                        }
                        document.getElementById('retorno').innerHTML = dadosObj.produtosLimpos.length
                        if (dadosObj.produtosLimpos.length == 0) {

                            linhas = `
                            <div class="table-row table-head">
                                <div class="table-cell first-cell">
                                    <p>Id</p>
                                </div>
                                <div class="table-cell">
                                    <p>Código de Referência</p>
                                </div>
                                <div class="table-cell table-cell-descricao">
                                    <p>Descrição</p>
                                </div>
                                <div class="table-cell last-cell headacoes">
                                    <p>Açoes</p>
                                </div>
                            </div>
                            <div class="table-row">
                                <div class="table-cell first-cell">
                                        <p>sem resultados.</p>
                                </div>
                            </div>
                            `
                        }

                        tbodyProdutos.innerHTML = linhas;
                    }
                    else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Oops...',
                            text: 'Informe no minimo 4 caracteres!'
                        })
                    }

                })
                .catch(function () {
                    tbodyIndustrias.innerHTML = `
                                            <div class="table-row">
                                                <div class="table-cell first-cell">
                                                        <p>deu erro.</p>
                                                </div>
                                            </div>
                                            `
                })
                .finally(function () {
                    document.getElementById("iconsearch").disabled = false;
                });
        }
        else {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Informe no minimo 4 caracteres!'
            })
        }
    }

}

document.getElementById("produto")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            indexListar.btnPesquisarOnClick();
        }
    });