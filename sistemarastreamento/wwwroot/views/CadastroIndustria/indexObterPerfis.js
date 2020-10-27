var indexObterPerfis = {

    btnPesquisarOnClick: function () {

        document.getElementById("tbPerfis").style.display = "table";

        var tbodyPerfis = document.getElementById("tbodyPerfis");
        tbodyPerfis.innerHTML = `<tr><td colspan="3"><img src=\"/img/ajax-loader.gif"\ /> carregando...</td></tr>`
        document.getElementById("btnPesquisar").disabled = "disabled";

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

                var linhas = "";
                for (var i = 0; i < dadosObj.length; i++) {

                    var template =
                        `<tr data-id="${dadosObj[i].id}">
                            <td>${dadosObj[i].nome}</td>
                            <td>${dadosObj[i].descricao}</td>                  
                            <td>
					            <a href="javascript:;" onclick="indexObterPerfis.selecionar(${dadosObj[i].id},'${dadosObj[i].nome}')" style="text-decoration:none">
                                    <img class="fa fa-fw fa-check-circle" />
                                </a>
                            </td>  
                         </tr>`
                    linhas += template;
                }

                if (linhas == "") {

                    linhas = `<tr><td colspan="3">Sem resultado.</td></tr>`
                }

                tbodyPerfis.innerHTML = linhas;
            })
            .catch(function () {
                tbodyPerfis.innerHTML = `<tr><td colspan="3">Deu erro...</td></tr>`
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