//parte de selecinar o tipo da pesquisa
$(".dropdown").click(function () {
    if ($(".dropdown ul").hasClass("active"))
        $(".dropdown ul").removeClass("active");
    else
        $(".dropdown ul").addClass("active");
});

$(".dropdown ul li").click(function () {
    var text = $(this).text();
    if (text == "Data") {
        $('#numero').mask('00/00/0000')
    } 
    else {

        $('#numero').mask('00000000000000000000000000000000000000000000')
    }
    $(".default_option").text(text);
    $(".default_option ul").removeClass("active");
});

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

                fetch("/LancamentoDist/Excluir?id=" + id, config)
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
                    'Nota Deletado.',
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
                    document.getElementById(id).remove()
                }
            })
            .catch(function () {
                alert("Deu erro.")
            })
    },

    btnPesquisarOnClick: function (perfil, id_dist) {

        var tbodyNotas = document.getElementById("table");
        var numero = document.getElementById("numero").value;

        if (numero.trim() != "") {

            tbodyNotas.innerHTML = `
                                   <div class="table-row table-head">
                                        <div class="table-cell first-cell">
                                            <p>Id</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Data</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Número</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Série</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Valor da Nota</p>
                                        </div>
                                        <div class="table-cell last-cell headacoes">
                                            <p>Açoes</p>
                                        </div>
                                    </div>
                                    `
            tbodyNotas.innerHTML += `
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

            fetch("/LancamentoDist/PesquisarDist?id_dist=" + id_dist + "&numero=" + numero + "&tipo=" + tipo, config)
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
                                    <p>Data</p>
                                </div>
                                <div class="table-cell">
                                    <p>Número</p>
                                </div>
                                <div class="table-cell">
                                    <p>Série</p>
                                </div>
                                <div class="table-cell">
                                    <p>Valor da Nota</p>
                                </div>
                                <div class="table-cell last-cell headacoes">
                                    <p>Açoes</p>
                                </div>
                            </div>
                            `;
                        for (var i = 0; i < dadosObj.notasLimpos.length; i++) {

                            var template =
                                `
                        <div class="table-row" id="${dadosObj.notasLimpos[i].id}">
                            <div class="table-cell first-cell">
                                <p>${dadosObj.notasLimpos[i].id}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.notasLimpos[i].data}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.notasLimpos[i].numero}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.notasLimpos[i].serie}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj.notasLimpos[i].valor_nf}</p>
                            </div>
                            <div class="table-cell last-cell acoes">
                                <div class="dropdown" onclick="indexListar.block(${dadosObj.notasLimpos[i].id})">
                                    <i class="dropbtn fa fa-fw fa-ellipsis-v"></i>
                                    <div class="dropdown-content" id="acoesoption${dadosObj.notasLimpos[i].id}" style="display: none;">
                                        <a data-fancybox data-type="iframe" data-src="/LancamentoDist/IndexVisualizar?id=${dadosObj.notasLimpos[i].id}" href="javascript:;"><i class='bx bx-show-alt'></i> Visualizar</a>
                                        <a href="javascript:indexListar.excluir(${dadosObj.notasLimpos[i].id})"><i class='bx bxs-user-x'></i> Excluir</a>
                                  </div>
                                </div>
                            </div>
                        </div>
                        `

                            linhas += template;
                        }
                        document.getElementById('retorno').innerHTML = dadosObj.notasLimpos.length
                        if (dadosObj.notasLimpos.length == 0) {

                            linhas = `
                            <div class="table-row table-head">
                                <div class="table-cell first-cell">
                                    <p>Id</p>
                                </div>
                                <div class="table-cell">
                                    <p>Data</p>
                                </div>
                                <div class="table-cell">
                                    <p>Número</p>
                                </div>
                                <div class="table-cell">
                                    <p>Série</p>
                                </div>
                                <div class="table-cell">
                                    <p>Valor da Nota</p>
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

                        tbodyNotas.innerHTML = linhas;
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
                    tbodyNotas.innerHTML = "";
                    tbodyNotas.innerHTML += `
                                        <div class="table-row table-head">
                                            <div class="table-cell first-cell">
                                                <p>Id</p>
                                            </div>
                                            <div class="table-cell">
                                                <p>Data</p>
                                            </div>
                                            <div class="table-cell">
                                                <p>Número</p>
                                            </div>
                                            <div class="table-cell">
                                                <p>Série</p>
                                            </div>
                                            <div class="table-cell">
                                                <p>Valor da Nota</p>
                                            </div>
                                            <div class="table-cell last-cell headacoes">
                                                <p>Açoes</p>
                                            </div>
                                        </div>
                                        <div class="table-row">
                                            <div class="table-cell first-cell">
                                                    <p>deu erro.</p>
                                            </div>
                                        </div>
                                            `
                })
                .finally(function () {

                    document.getElementById("iconsearch").disabled = "";
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

document.getElementById("numero")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            indexListar.btnPesquisarOnClick();
        }
    });