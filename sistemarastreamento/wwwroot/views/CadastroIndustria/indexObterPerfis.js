var indexObterPerfis = {

    btnPesquisarOnClick: function () {

        var tbodyPerfis = document.getElementById("table");
     
        document.getElementById("btnPesquisar").disabled = "disabled";

        tbodyPerfis.innerHTML = `
                                   <div class="table-row table-head">
                                        <div class="table-cell first-cell">
                                            <p>Função</p>
                                        </div>
                                        <div class="table-cell">
                                            <p>Descrição</p>
                                        </div>
                                        <div class="table-cell last-cell headacoes">
                                            <p>Ação</p>
                                        </div>
                                    </div>
                                    `
        tbodyPerfis.innerHTML += `
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

        var nome = encodeURIComponent(document.getElementById("nome").value);

        fetch("/CadastroIndustria/ObterPerfis?nome=" + nome, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                var linhas = `
                            <div class="table-row table-head">
                                <div class="table-cell first-cell">
                                    <p>Função</p>
                                </div>
                                <div class="table-cell">
                                    <p>Descrição</p>
                                </div>
                                <div class="table-cell last-cell headacoes">
                                    <p>Ação</p>
                                </div>
                            </div>
                            `;

                for (var i = 0; i < dadosObj.length; i++) {

                    var template =
                        `
                        <div class="table-row">
                            <div class="table-cell first-cell">
                                <p>${dadosObj[i].nome}</p>
                            </div>
                            <div class="table-cell">
                                <p>${dadosObj[i].descricao}</p>
                            </div>
                            <div class="table-cell last-cell acoes">
                                <a href="javascript:;" onclick="indexObterPerfis.selecionar(${dadosObj[i].id},'${dadosObj[i].nome}')" style="text-decoration:none">
                                    <img class="fa fa-fw fa-check-circle" />
                                </a>
                            </div>
                        </div>
                        `

                    linhas += template;
                }

                if (dadosObj.length == 0) {

                    linhas = `
                            <div class="table-row table-head">
                                <div class="table-cell first-cell">
                                    <p>Função</p>
                                </div>
                                <div class="table-cell">
                                    <p>Descrição</p>
                                </div>
                                <div class="table-cell last-cell headacoes">
                                    <p>Ação</p>
                                </div>
                            </div>
                            <div class="table-row">
                                <div class="table-cell first-cell">
                                        <p>sem resultados.</p>
                                </div>
                            </div>
                            `
                }

                tbodyPerfis.innerHTML = linhas;
            })
            .catch(function () {
                tbodyPerfis.innerHTML = "";
                tbodyPerfis.innerHTML += `
                                        <div class="table-row table-head">
                                            <div class="table-cell first-cell">
                                                <p>Função</p>
                                            </div>
                                            <div class="table-cell">
                                                <p>Descrição</p>
                                            </div>
                                            <div class="table-cell last-cell headacoes">
                                                <p>Ação</p>
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

                document.getElementById("btnPesquisar").disabled = "";
            });
        //<td><a href="javascript:indexListar.excluir(${dadosObj[i].id})">Excluir</a></td> 
    },

    selecionar: function (id, nome) {
        window.parent.index.selecionarPerfil(id, nome);
    }

}

document.getElementById("nome")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            indexObterPerfis.btnPesquisarOnClick();
        }
    });
