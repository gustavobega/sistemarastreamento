var indexListar = {

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

                fetch("/CadastroProdDist/Excluir?id=" + id, config)
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

    btnPesquisarOnClick: function (perfil) {
        document.getElementById("iconsearch").disabled = true;

        var tbodyProdutos = document.getElementById("table");
        tbodyProdutos.innerHTML = `
                                   <div class="table-row table-head">
                                        <div class="table-cell first-cell">
                                            <p>Código de Referência</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Código</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Descrição</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Lote</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Saldo</p>
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
        var descricao = encodeURIComponent(document.getElementById("descricao").value);

        fetch("/CadastroProdDist/Pesquisar?descricao=" + descricao + "&tipo=" + tipo, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                var linhas = ` 
                        <div class="table-row table-head">
                            <div class="table-cell first-cell">
                                <p>Código de Referência</p>
                            </div>
                            <div class="table-cell">
                                <p>Código</p>
                            </div>
                            <div class="table-cell">
                                <p>Descrição</p>
                            </div>
                            <div class="table-cell">
                                <p>Lote</p>
                            </div>
                            <div class="table-cell">
                                <p>Saldo</p>
                            </div>
                            <div class="table-cell last-cell headacoes">
                                <p>Açoes</p>
                            </div>
                        </div>
                        `;
                var lista = dadosObj.produtosLimpos;
                for (var i = 0; i < lista.length; i++) {

                    var template =
                        `
                        <div class="table-row" id="${lista[i]['id_estoque']}">
                            <div class="table-cell first-cell">
                                <p>${lista[i]['cod_ref']}</p>
                            </div>
                            <div class="table-cell">
                                <p>${lista[i]['codigo']}</p>
                            </div>
                            <div class="table-cell">
                                <p>${lista[i]['descricao']}</p>
                            </div>
                            <div class="table-cell">
                                <p>${lista[i]['lote']}</p>
                            </div>
                            <div class="table-cell">
                                <p>${lista[i]['saldo']}</p>
                            </div>
                            <div class="table-cell last-cell acoes">
                                <div class="dropdown" onclick="indexListar.block(${lista[i]['id_estoque']})">
                                    <i class="dropbtn fa fa-fw fa-ellipsis-v"></i>
                                    <div class="dropdown-content" id="acoesoption${lista[i]['id_estoque']}" style="display: none;">
                                        <a href="/CadastroProdDist/Editar?id=${lista[i]['id_estoque']}"><i class='bx bx-edit'></i> Editar</a>
                                        <a href="javascript:indexListar.excluir(${lista[i]['id_estoque']})"><i class='bx bxs-user-x'></i> Excluir</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        `

                    linhas += template;
                }
                document.getElementById('retorno').innerHTML = lista.length
                if (lista.length == 0) {

                    linhas = `
                            <div class="table-row table-head">
                                <div class="table-cell first-cell">
                                    <p>Código de Referência</p>
                                </div>
                                <div class="table-cell">
                                    <p>Código</p>
                                </div>
                                <div class="table-cell">
                                    <p>Descrição</p>
                                </div>
                                <div class="table-cell">
                                    <p>Lote</p>
                                </div>
                                <div class="table-cell">
                                    <p>Saldo</p>
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
            })
            .catch(function () {
                tbodyProdutos.innerHTML += `
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
        //<td><a href="javascript:indexListar.excluir(${dadosObj[i].id})">Excluir</a></td> 
    }

}

document.getElementById("descricao")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            indexListar.btnPesquisarOnClick();
        }
    });