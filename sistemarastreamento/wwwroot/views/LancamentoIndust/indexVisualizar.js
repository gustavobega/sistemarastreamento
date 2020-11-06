var index = {

    obterInfoNota: function () {

        id_nota = document.getElementById("hfIdNota").value;
        var tbodyItens = document.getElementById("table");

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/LancamentoIndust/carregaItensNota?id=" + id_nota, config)

            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                var linhas = "";
                var obj = JSON.parse(dadosObj);
                for (var i = 0; i < obj.length; i++) {

                    var template =
                        `
                        <div class="table-row">
                            <div class="table-cell first-cell">
                                <p>${obj[i].cod_ref}</p>
                            </div>
                            <div class="table-cell">
                                <p>${obj[i].descricao}</p>
                            </div>
                            <div class="table-cell">
                                <p>${obj[i].lote}</p>
                            </div>
                            <div class="table-cell">
                                <p>${obj[i].qtde}</p>
                            </div>

                            <div class="table-cell last-cell">
                                <p>R$${obj[i].valor_unit}</p>
                            </div>

                        </div>
                        `

                    linhas += template;

                }
                tbodyItens.innerHTML += linhas;

            })
            .catch(function () {
                alert("Deu erro.")
            })
    }
}
//para chamar a função JS logo apos o carregamento da página
index.obterInfoNota();
