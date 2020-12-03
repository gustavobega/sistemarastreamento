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

                fetch("/CadastroDistribuidor/Excluir?id=" + id, config)
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
                    'Distribuidor Deletado.',
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
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/CadastroDistribuidor/Alterar?id=" + id, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                if (dadosObj.operacao) {
                    alert(dadosObj.distribuidor.nome);
                }
            })
            .catch(function () {
                alert("Deu erro.")
            })
    },
    
    btnPesquisarOnClick: function (perfil) {

        document.getElementById("iconsearch").disabled = true;
        var nome = encodeURIComponent(document.getElementById("distribuidor").value);
        var tbodyDistribuidores = document.getElementById("table");

        if (nome.trim() != "") {


            tbodyDistribuidores.innerHTML = `
                                   <div class="table-row table-head">
                                        <div class="table-cell first-cell">
                                            <p>Id</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Nome</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>CNPJ</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Telefone</p>
                                        </div>
                                        <div class="table-cell last-cell">
                                            <p>Açoes</p>
                                        </div>
                                    </div>
                                    `
            tbodyDistribuidores.innerHTML += `
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

            fetch("/CadastroDistribuidor/Pesquisar?nome=" + nome + "&tipo=" + tipo, config)
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
                                <p>Nome</p>
                            </div>
                            <div class="table-cell">
                                <p>CNPJ</p>
                            </div>
                            <div class="table-cell">
                                <p>Telefone</p>
                            </div>
                            <div class="table-cell last-cell">
                                <p>Açoes</p>
                            </div>
                        </div>
                        `;

                        for (var i = 0; i < dadosObj.distribuidoresLimpos.length; i++) {

                            var template =
                                `
                        <div class="table-row" id="${dadosObj.distribuidoresLimpos[i].id}">
                            <div class="table-cell first-cell">
                                <p>${dadosObj.distribuidoresLimpos[i].id}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.distribuidoresLimpos[i].nome}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.distribuidoresLimpos[i].cnpj}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.distribuidoresLimpos[i].telefone}</p>
                            </div>
                            <div class="table-cell last-cell">
                                <div class="dropdown" onclick="indexListar.block(${dadosObj.distribuidoresLimpos[i].id})">
                                    <i class="dropbtn fa fa-fw fa-ellipsis-v"></i>
                                    <div class="dropdown-content" id="acoesoption${dadosObj.distribuidoresLimpos[i].id}" style="display: none;">
                                        <a data-fancybox data-type="iframe" data-src="/CadastroDistribuidor/IndexVisualizar?id=${dadosObj.distribuidoresLimpos[i].id}" href="javascript:;"><i class='bx bx-show-alt'></i> Visualizar</a>                       
                                        <a href="/CadastroDistribuidor/Editar?id=${dadosObj.distribuidoresLimpos[i].id}"><i class='bx bx-edit'></i> Editar</a>
                                        <a data-fancybox data-type="iframe" data-src="/CadastroDistribuidor/IndexEmail?emaildist=${dadosObj.distribuidoresLimpos[i].email}" href="javascript:;"><i class='bx bx-send'></i> Enviar E-mail</a>
                                        <a data-fancybox data-type="iframe" data-src="/CadastroDistribuidor/IndexSMS?telldist=${dadosObj.distribuidoresLimpos[i].telefone}" href="javascript:;"><i class='bx bx-mail-send'></i> Enviar SMS</a>
                                        <a href="javascript:indexListar.excluir(${dadosObj.distribuidoresLimpos[i].id})"><i class='bx bxs-user-x'></i> Excluir</a>
                                  </div>
                                </div>
                            </div>
                        </div>
                        `

                            linhas += template;
                        }
                        document.getElementById('retorno').innerHTML = dadosObj.distribuidoresLimpos.length
                        if (dadosObj.distribuidoresLimpos.length == 0) {

                            linhas = `
                            <div class="table-row table-head">
                                <div class="table-cell first-cell">
                                    <p>Id</p>
                                </div>
                                <div class="table-cell">
                                    <p>Nome</p>
                                </div>
                                <div class="table-cell">
                                    <p>CNPJ</p>
                                </div>
                                <div class="table-cell">
                                    <p>Telefone</p>
                                </div>
                                <div class="table-cell last-cell">
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

                        tbodyDistribuidores.innerHTML = linhas;
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
                    tbodyDistribuidores.innerHTML = `
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

document.getElementById("distribuidor")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            indexListar.btnPesquisarOnClick();
        }
    });