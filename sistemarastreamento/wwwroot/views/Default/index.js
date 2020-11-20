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

                document.getElementById('notas').classList.add("active")
                var estoque = document.getElementById('estoque');
                if (estoque != null) {
                    estoque.classList.remove("active")
                }
                document.getElementById('ganho').classList.remove("active");

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

                document.getElementById('dados').style.display = 'none'
                document.getElementById('graficoAno').style.display = 'none'

                document.getElementById('dadosGanho').style.display = 'none'
                document.getElementById('graficoGanho').style.display = 'none'

                document.getElementById('graficoEstoque').style.display = 'block'
                document.getElementById('dadosEstoque').style.display = 'block'
           
                document.getElementById('estoque').classList.add("active")
                document.getElementById('notas').classList.remove("active")
                document.getElementById('ganho').classList.remove("active");
                var ctx = document.getElementById('graficoEstoque')

                new Chart(ctx, {
                    type: 'doughnut',
                    data: {
                        labels: dadosObj.produto,
                        datasets: [{
                            data: dadosObj.saldo,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.8)',
                                'rgba(54, 162, 235, 0.8)',
                                'rgba(255, 206, 86, 0.8)',
                                'rgba(75, 192, 192, 0.8)',
                                'rgba(153, 102, 255, 0.8)',
                                'rgba(255, 159, 64, 0.8)'
                            ],
                            borderColor: [
                                'rgba(255, 99, 132, 1.2)',
                                'rgba(54, 162, 235, 1.2)',
                                'rgba(255, 206, 86, 1.2)',
                                'rgba(75, 192, 192, 1.2)',
                                'rgba(153, 102, 255, 1.2)',
                                'rgba(255, 159, 64, 1.2)'
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
            })
    },
    geraGraficoGanho: function () {
        var ano = document.getElementById('ano').value;

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
                    estoque.classList.remove("active")
                }
                document.getElementById('notas').classList.remove("active");
                document.getElementById('ganho').classList.add("active")

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

index.geraGraficoAno();