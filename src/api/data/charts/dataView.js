//import 'chartjs-adapter-date-fns'
//const Chart = require('chart.js'); if running in Node.js
//Fetch data

const scale = 15;

const url = `http://192.168.15.9:5000/monitor/getLevels?n=${scale}`;
const varUrl = `http://192.168.15.9:5000/monitor/variation?fromLast=${scale}`;
const freqUrl = "http://192.168.15.9:5000/monitor/frequency";

let scl = document.getElementById('scale');
scl.textContent = `${scale} measures`;

fetch(varUrl)
    .then(response => response.json())
    .then(data => {
        const variation = Math.round(parseFloat(data) * 100) / 100;
        let varLabel = document.getElementById('variation');
        varLabel.textContent = `${variation} units`;
        console.log(`variation: ${variation}`)
    })
    .catch(error => console.error('Error:', error));


fetch(freqUrl)
    .then(response => response.json())
    .then(data => {
        const frequency = Math.round(parseFloat(data) * 100);
        let freqLabel = document.getElementById('freq');
        freqLabel.textContent = `${frequency}%`;
        console.log(`frequency: ${frequency}`)
    })
    .catch(error => console.error('Error:', error));

fetch(url)
    .then(response => response.json())
    .then(data => {
        //Get the waterlevel and time data from the json
        const WaterLevels = data.map(item => item.waterLevel);  
        const RecordTimes = data.map(item => item.timeOfRecord);
        console.log(WaterLevels);
        console.log(RecordTimes);
        console.log(data);


        //Create chart
        //Get chart canvas element in page.html
        const ctx = document.getElementById('dataChart').getContext('2d'); 

        //Creates a chart whilst defining type, desired values, color... etc
        new Chart(ctx, {
            type: 'line',
            data: {
                labels: RecordTimes,
                datasets: [{
                    label: 'Water Level',
                    data: WaterLevels,
                    fill: false,
                    borderColor: 'rgb(75, 192, 192)',
                    tension: 0
                }]
            },
            options: {
                scales: {
                    x: {
                        type: 'time',
                        time: {
                            unit: 'minute',
                        },
                        title:{
                            display: true,
                            text: 'Time'
                        }
                    },
                    y:{
                        display: true,
                        title:{
                            display: true,
                            text: 'Water Level'
                        },
                        ticks:{
                            min: 0.0,
                            max: 10.0
                        }
                    }
                },
                responsive: false,
                width: 800,
                height: 400
            }
        })
    })