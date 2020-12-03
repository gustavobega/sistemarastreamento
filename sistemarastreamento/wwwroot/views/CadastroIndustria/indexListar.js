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

                fetch("/CadastroIndustria/Excluir?id=" + id, config)
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
                    'Indústria Deletado.',
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

        fetch("/CadastroIndustria/Alterar?id=" + id, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                alert(dadosObj.industria.nome);

            })
            .catch(function () {
                alert("Deu erro.")
            })
    },

    btnPesquisarOnClick: function (perfil) {
        document.getElementById("iconsearch").disabled = true;
        var tbodyIndustrias = document.getElementById("table");
        var nome = encodeURIComponent(document.getElementById("industria").value);

        if (nome.trim() != "") {

            tbodyIndustrias.innerHTML = `
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
            tbodyIndustrias.innerHTML += `
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

            fetch("/CadastroIndustria/Pesquisar?nome=" + nome + "&tipo=" + tipo, config)
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

                        for (var i = 0; i < dadosObj.industriasLimpos.length; i++) {

                            var template =
                                `
                        <div class="table-row" id="${dadosObj.industriasLimpos[i].id}">
                            <div class="table-cell first-cell">
                                <p>${dadosObj.industriasLimpos[i].id}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.industriasLimpos[i].nome}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.industriasLimpos[i].cnpj}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.industriasLimpos[i].telefone}</p>
                            </div>
                            <div class="table-cell last-cell">
                                <div class="dropdown" onclick="indexListar.block(${dadosObj.industriasLimpos[i].id})">
                                    <i class="dropbtn fa fa-fw fa-ellipsis-v"></i>
                                    <div class="dropdown-content" id="acoesoption${dadosObj.industriasLimpos[i].id}" style="display: none;">
                                        <a data-fancybox data-type="iframe" data-src="/CadastroIndustria/IndexVisualizar?id=${dadosObj.industriasLimpos[i].id}" href="javascript:;"><i class='bx bx-show-alt'></i> Visualizar</a>
                                        <a href="/CadastroIndustria/Editar?id=${dadosObj.industriasLimpos[i].id}"><i class='bx bx-edit'></i> Editar</a>
                                        <a href="javascript:indexListar.excluir(${dadosObj.industriasLimpos[i].id})"><i class='bx bxs-user-x'></i> Excluir</a>
                                  </div>
                                </div>
                            </div>
                        </div>
                        `

                            linhas += template;
                        }
                        document.getElementById('retorno').innerHTML = dadosObj.industriasLimpos.length
                        if (dadosObj.industriasLimpos.length == 0) {

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

                        tbodyIndustrias.innerHTML = linhas;
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

document.getElementById("industria")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            indexListar.btnPesquisarOnClick();
        }
    });