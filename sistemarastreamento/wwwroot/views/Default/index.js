var index = {
    geraGraficoAno: function () {
        var ano = document.getElementById('ano').value;

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/Default/geraGraficoAno?ano=" + ano, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                document.getElementById('dados').style.display = 'block'
                document.getElementById('graficoAno').style.display = 'block'

                document.getElementById('graficoEstoque').style.display = 'none'
                document.getElementById('dadosEstoque').style.display = 'none'

                document.getElementById('graficoGanho').style.display = 'none'
                document.getElementById('dadosGanho').style.display = 'none'

                document.getElementById('notas').classList.add("selecionado")
                var estoque = document.getElementById('estoque');
                if (estoque != null) {
                    estoque.classList.remove("selecionado")
                }
                document.getElementById('ganho').classList.remove("selecionado");

                var ctx = document.getElementById('graficoAno')
                new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                        datasets: [{
                            data: dadosObj,
                            backgroundColor: 'rgba(54, 162, 235, 0.2)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });
            })           
    }
    ,
    geraGraficoEstoque: function () {

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/Default/geraGraficoEstoque", config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {

                var width = window.innerWidth;

                document.getElementById('dados').style.display = 'none'
                document.getElementById('graficoAno').style.display = 'none'

                document.getElementById('dadosGanho').style.display = 'none'
                document.getElementById('graficoGanho').style.display = 'none'

                document.getElementById('graficoEstoque').style.display = 'block'
                document.getElementById('dadosEstoque').style.display = 'block'
           
                document.getElementById('estoque').classList.add("selecionado")
                document.getElementById('notas').classList.remove("selecionado")
                document.getElementById('ganho').classList.remove("actselecionadoive");
                var ctx = document.getElementById('graficoEstoque')

                if (width > 600) {
                    new Chart(ctx, {
                        type: 'pie',
                        data: {
                            labels: dadosObj.produto,
                            datasets: [{
                                data: dadosObj.saldo,
                                backgroundColor: [
                                    'rgba(255, 99, 132, 0.2)',
                                    'rgba(54, 162, 235, 0.2)',
                                    'rgba(255, 206, 86, 0.2)',
                                    'rgba(75, 192, 192, 0.2)',
                                    //'rgba(255,159,64, 0.2)',
                                    'rgba(153,102,255, 0.2)',

                                ],
                                borderColor: [
                                    'rgba(255,99,132, 1)',
                                    'rgba(54, 162, 235, 1)',
                                    'rgba(255, 206, 86, 1)',
                                    'rgba(75, 192, 192, 1)',
                                    //'rgba(255,159,64, 1)',
                                    'rgba(153,102,255, 1)',
                                ],
                                borderWidth: 1
                            }],
                        },
                        options: {

                            legend: {
                                display: true,
                                position: 'left',
                                align: 'start',
                                labels: {
                                    fontSize: 13,
                                    padding: 15
                                }
                            }
                        }
                    });
                }
                else {
                    new Chart(ctx, {
                        type: 'pie',
                        data: {
                            labels: dadosObj.produto,
                            datasets: [{
                                data: dadosObj.saldo,
                                backgroundColor: [
                                    'rgba(255, 99, 132, 0.2)',
                                    'rgba(54, 162, 235, 0.2)',
                                    'rgba(255, 206, 86, 0.2)',
                                    'rgba(75, 192, 192, 0.2)',
                                    //'rgba(255,159,64, 0.2)',
                                    'rgba(153,102,255, 0.2)',

                                ],
                                borderColor: [
                                    'rgba(255,99,132, 1)',
                                    'rgba(54, 162, 235, 1)',
                                    'rgba(255, 206, 86, 1)',
                                    'rgba(75, 192, 192, 1)',
                                    //'rgba(255,159,64, 1)',
                                    'rgba(153,102,255, 1)',
                                ],
                                borderWidth: 1
                            }],
                        },
                        options: {

                            legend: {
                                display: false
                            }
                        }
                    });
                }
                
            })
    },
    geraGraficoGanho: function () {
        var ano = document.getElementById('anoGanho').value;

        var config = {
            method: "GET",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            credentials: 'include', //inclui cookies
        };

        fetch("/Default/geraGraficoGanho?ano=" + ano, config)
            .then(function (dadosJson) {
                var obj = dadosJson.json(); //deserializando
                return obj;
            })
            .then(function (dadosObj) {
                document.getElementById('dados').style.display = 'none'
                document.getElementById('graficoAno').style.display = 'none'

                document.getElementById('graficoEstoque').style.display = 'none'
                document.getElementById('dadosEstoque').style.display = 'none'

                document.getElementById('graficoGanho').style.display = 'block'
                document.getElementById('dadosGanho').style.display = 'block'

                var estoque = document.getElementById('estoque');
                if (estoque != null) {
                    estoque.classList.remove("selecionado")
                }
                document.getElementById('notas').classList.remove("selecionado");
                document.getElementById('ganho').classList.add("selecionado")

                var ctx = document.getElementById('graficoGanho')
                new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                        datasets: [{
                            data: dadosObj,
                            backgroundColor: 'rgba(54, 162, 235, 0.2)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }]
                        }
                    }
                });
            })
    }
}

document.getElementById("ano")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            index.geraGraficoAno();
        }
    });

document.getElementById("anoGanho")
    .addEventListener("keyup", function (event) {
        event.preventDefault();
        if (event.keyCode === 13) {
            index.geraGraficoGanho();
        }
    });

index.geraGraficoAno();