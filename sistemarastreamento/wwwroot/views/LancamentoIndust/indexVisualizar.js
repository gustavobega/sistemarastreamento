var index = {

    obterInfoNota: function () {

        id_nota = document.getElementById("hfIdNota").value;
        var tbodyItens = document.getElementById("tbodyItens");
        tbodyItens.innerHTML = `<tr><td colspan="3"><img src=\"/img/ajax-loader.gif"\ /> carregando...</td></tr>`

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
                document.getElementById("tbItens").style.display = "table";

                var linhas = "";
                var obj = JSON.parse(dadosObj);
                for (var i = 0; i < obj.length; i++) {

                    var template =
                        `<tr>
                            <td>${obj[i].cod_ref}</td>
                            <td>${obj[i].descricao}</td>
                            <td>${obj[i].lote}</td>
                            <td>${obj[i].qtde}</td>
                            <td>R$${obj[i].valor_unit}</td> `

                    template += "</td> </tr>";

                    linhas += template;
                }
                tbodyItens.innerHTML = linhas;

            })
            .catch(function () {
                alert("Deu erro.")
            })
    }
}
//para chamar a função JS logo apos o carregamento da página
index.obterInfoNota();
