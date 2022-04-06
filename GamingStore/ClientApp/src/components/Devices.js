import React, { Component } from 'react';
import { DeviceRow } from './DeviceRow';

export class Devices extends Component {
    static displayName = Devices.name;

    constructor(props) {
        super(props);

        this.state = {
            devices: []
        };

        this.getDevices = this.getDevices.bind(this);
    }

    async componentDidMount() {
        await this.getDevices();
    }

    async getDevices() {
        await fetch('../api/devices', {
            method: 'GET'
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }

                return response.text().then(error => { throw new Error(error) });
            })
            .then(data => this.setState({devices: data}))
            .catch(error => alert(error.message));
    }

    handleDevices = devices => {
        this.setState({devices: devices});
    }

    render() {
        return <div>
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Id</th>
                        <th className="text-center">Name</th>
                        <th className="text-center">Category</th>
                        <th className="text-center">Price</th>
                        <th className="text-center">In Stock</th>
                        <th className="text-center">CompanyId</th>
                        <th className="text-center">Actions</th>
                    </tr>
                </thead>
                <tbody>
                {
                    this.state.devices.map(device => {
                        return <DeviceRow onGetDevices={this.handleDevices}
                                        key={device.id}
                                        id={device.id}
                                        name={device.name}
                                        category={device.category}
                                        price={device.price}
                                        inStock={device.inStock}
                                        companyId={device.companyId}
                                        getDevices={this.getDevices} />
                    })
                }
                </tbody>
            </table>

            <a className="btn btn-secondary w-100" href="/devices/add">Add</a>
        </div>;
    }
}