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

        var config = {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/LancamentoIndust/Excluir?id=" + id, config)
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

    alterar: function (id) {

        var config = {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/CadastroIndustria/Alterar?id=" + id, config)
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

    btnPesquisarOnClick: function (perfil,id_indust) {

        var tbodyNotas = document.getElementById("table");
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

        var tipo = $(".default_option").text();
        var numero = encodeURIComponent(document.getElementById("numero").value);

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/LancamentoIndust/PesquisarIndust?id_indust=" + id_indust + "&numero= " + numero + "&tipo=" + tipo, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

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
                
                for (var i = 0; i < dadosObj.length; i++) {

                    var template =
                        `
                        <div class="table-row" id="${dadosObj[i].id}">
                            <div class="table-cell first-cell">
                                <p>${dadosObj[i].id}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj[i].data}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj[i].numero}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj[i].serie}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj[i].valor_nf}</p>
                            </div>
                            <div class="table-cell last-cell acoes">
                                <div class="dropdown" onclick="indexListar.block(${dadosObj[i].id})">
                                    <i class="dropbtn fa fa-fw fa-ellipsis-v"></i>
                                    <div class="dropdown-content" id="acoesoption${dadosObj[i].id}" style="display: none;">
                                        <a data-fancybox data-type="iframe" data-src="/LancamentoIndust/IndexVisualizar?id=${dadosObj[i].id}" href="javascript:;"><i class='bx bx-show-alt'></i> Visualizar</a>
                                        <a href="javascript:indexListar.excluir(${dadosObj[i].id})" onclick="return confirm('Confirmar Exclusão?')"><i class='bx bxs-user-x'></i> Excluir</a>
                                  </div>
                                </div>
                            </div>
                        </div>
                        `
     
                    linhas += template;
                }

                if (dadosObj.length == 0) {

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
        //<td><a href="javascript:indexListar.excluir(${dadosObj[i].id})">Excluir</a></td> 
    }

}

document.getElementById("numero")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            indexListar.btnPesquisarOnClick();
        }
    });