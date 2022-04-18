import React, { Component } from 'react';
import { formErrorMessage } from '../modules/errorMessageFormer';


export class AddCompanyForm extends Component {
    static displayName = AddCompanyForm.name;

    constructor(props) {
        super(props);

        this.state = {
            name: '',
            icon: [],
            token: sessionStorage.getItem('token')
        };

        this.addCompany = this.addCompany.bind(this);
        this.handleName = this.handleName.bind(this);
        this.handleFileChange = this.handleFileChange.bind(this);
    }

    async addCompany(event) {
        event.preventDefault();

        await fetch('../api/companies', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.state.token}`
            },
            body: JSON.stringify({
                ...this.state,
                icon: JSON.stringify(this.state.icon)
            })
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }

                return formErrorMessage(response);
            })
            .then(() => window.location.href = '/companies')
            .catch(error => alert(error.message));
    }

    handleName = event => {
        this.setState({name: event.target.value});
    }

    handleFileChange = event => {
        const reader = new FileReader();
        const fileByteArray = [];

        reader.readAsArrayBuffer(event.target.files[0]);
        reader.onloadend = evt => {
            if (evt.target.readyState === FileReader.DONE) {
                const arrayBuffer = evt.target.result;
                const array = new Uint8Array(arrayBuffer);

                for (let i = 0; i < array.length; i++) {
                    fileByteArray.push(array[i]);
                }

                this.setState({
                    icon: fileByteArray
                });
            }
        }
    }

    render() {
        return <form onSubmit={this.addCompany}>
            <div className='form-group my-3'>
                <label htmlFor='name' className='form-label'>Name:</label>
                <input id='name' className='form-control' type='text' value={this.state.name} onChange={this.handleName} />
            </div>
            <div className="form-group my-2">
                <label htmlFor="icon" className="form-label">Choose a picture:</label>
                <input id="icon" className="form-control" type="file" onChange={this.handleFileChange} />
            </div>

            <input className='btn btn-primary' type='submit' value='Create' />
        </form>;
    }
}